using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjektAPBD.Contexts;
using ProjektAPBD.Exceptions;
using ProjektAPBD.Models;

namespace ProjektAPBD.Services;

public interface IEmployeeService
{
    Task<string> Login(string username, string password);
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> Register(string username, string password, string role);
    Task<int> DeleteEmployee(int id);
}
public class EmployeeService : IEmployeeService
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _config;

    public EmployeeService(DatabaseContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> Login(string username, string password)
    {
        var employee = await _context.Employees.SingleOrDefaultAsync(x => x.Username == username);

        if (employee == null)
            throw new NotFoundException("Employee not found");

        if (!VerifyPasswordHash(password, employee.PasswordHash, employee.PasswordSalt))
            throw new ArgumentException("Invalid password");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, employee.Id.ToString()),
                new Claim(ClaimTypes.Role, employee.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> Register(string username, string password, string role)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Username, password and role cannot be empty");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

            var employeeExists = await _context.Employees.AnyAsync(e => e.Username == username);
            if (employeeExists)
            {
                throw new ArgumentException("Employee with that username already exists");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var employee = new Employee
            {
                Username = username,
                Role = role,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return employee;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> DeleteEmployee(int id)
    {
        if (id < 1)
        {
            throw new ArgumentException("Invalid id");
        }
        
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new NotFoundException("Employee not found");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return 1;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null)
        {
            throw new ArgumentException("Password cannot be null");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be empty or whitespace only string.");
        }

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null)
        {
            throw new ArgumentException("Password cannot be null");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be empty or whitespace only string.");
        }

        if (storedHash.Length != 64)
        {
            throw new ArgumentException("Invalid length of password hash (64 bytes expected).");
        }

        if (storedSalt.Length != 128)
        {
            throw new ArgumentException("Invalid length of password salt (128 bytes expected).");
        }

        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }
        }

        return true;
    }
}