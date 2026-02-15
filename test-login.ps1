# PowerShell script to test login and capture screenshot
$ErrorActionPreference = "Stop"

# Check if pwsh module for Playwright exists, otherwise use dotnet
Write-Host "Testing login flow..." -ForegroundColor Cyan

# Create a simple C# script to use Playwright
$csharpCode = @"
using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

class Program
{
    static async Task Main(string[] args)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
        var page = await browser.NewPageAsync();
        
        Console.WriteLine("=== Step 1: Navigate to login page ===");
        await page.GotoAsync("http://localhost:5234/login");
        Console.WriteLine($"Current URL: {page.Url}");
        
        Console.WriteLine("\n=== Step 2: Fill username ===");
        await page.FillAsync("#username", "admin");
        Console.WriteLine("Username filled: admin");
        
        Console.WriteLine("\n=== Step 3: Fill password ===");
        await page.FillAsync("#password", "password123");
        Console.WriteLine("Password filled: password123");
        
        Console.WriteLine("\n=== Step 4: Click login button ===");
        await page.ClickAsync("button[type='submit']");
        Console.WriteLine("Login button clicked");
        
        Console.WriteLine("\n=== Step 5: Wait 5 seconds ===");
        await Task.Delay(5000);
        
        Console.WriteLine("\n=== Step 6: Current state after 5 seconds ===");
        Console.WriteLine($"Current URL: {page.Url}");
        Console.WriteLine($"Page Title: {await page.TitleAsync()}");
        
        // Check if we're on customers page
        if (page.Url.Contains("/customers"))
        {
            Console.WriteLine("\n✓ Successfully navigated to /customers page");
            
            // Check for h1
            var h1 = await page.Locator("h1").TextContentAsync();
            Console.WriteLine($"Page Heading: {h1}");
            
            // Check for table
            var tableExists = await page.Locator("table").CountAsync() > 0;
            Console.WriteLine($"Table exists: {tableExists}");
            
            if (tableExists)
            {
                var rowCount = await page.Locator("tbody tr").CountAsync();
                Console.WriteLine($"Number of rows in table: {rowCount}");
                
                // Check for New Customer button
                var newCustomerBtn = await page.Locator("button.btn-primary:has(i.bi-plus-lg)").CountAsync() > 0;
                Console.WriteLine($"New Customer button exists: {newCustomerBtn}");
            }
            
            // Take screenshot
            await page.ScreenshotAsync(new() { Path = "customers-page.png", FullPage = true });
            Console.WriteLine("\n✓ Screenshot saved to: customers-page.png");
        }
        else
        {
            Console.WriteLine($"\n✗ NOT on customers page. Current URL: {page.Url}");
            
            // Check for error messages
            var errorExists = await page.Locator(".alert-danger").CountAsync() > 0;
            if (errorExists)
            {
                var errorText = await page.Locator(".alert-danger").TextContentAsync();
                Console.WriteLine($"Error message: {errorText}");
            }
            
            // Take screenshot of error
            await page.ScreenshotAsync(new() { Path = "error-page.png", FullPage = true });
            Console.WriteLine("\n✓ Screenshot saved to: error-page.png");
        }
        
        // Get console logs
        Console.WriteLine("\n=== Browser Console Logs ===");
        page.Console += (_, msg) => Console.WriteLine($"[{msg.Type}] {msg.Text}");
        
        Console.WriteLine("\nPress any key to close browser...");
        Console.ReadKey();
    }
}
"@

# Save C# code
Set-Content -Path "test-login-temp.cs" -Value $csharpCode

Write-Host "C# script created. Running with dotnet..." -ForegroundColor Yellow
