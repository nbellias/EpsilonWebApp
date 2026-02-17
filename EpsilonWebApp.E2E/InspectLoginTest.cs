using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace EpsilonWebApp.E2E
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class InspectLoginTest : PageTest
    {
        private const string BaseUrl = "http://localhost:5234";

        [Test]
        public async Task Inspect_Login_Flow_And_Customers_Page()
        {
            Console.WriteLine("=== Step 1: Navigate to login page ===");
            await Page.GotoAsync($"{BaseUrl}/login");
            Console.WriteLine($"Current URL: {Page.Url}");
            await Page.ScreenshotAsync(new() { Path = "screenshots/01-login-page.png", FullPage = true });

            Console.WriteLine("\n=== Step 2: Fill username ===");
            await Page.FillAsync("#username", "admin");
            Console.WriteLine("Username filled: admin");

            Console.WriteLine("\n=== Step 3: Fill password ===");
            await Page.FillAsync("#password", "password123");
            Console.WriteLine("Password filled: password123");
            await Page.ScreenshotAsync(new() { Path = "screenshots/02-credentials-filled.png", FullPage = true });

            Console.WriteLine("\n=== Step 4: Click login button ===");
            await Page.ClickAsync("button[type='submit']");
            Console.WriteLine("Login button clicked");

            Console.WriteLine("\n=== Step 5: Wait 5 seconds ===");
            await Task.Delay(5000);

            Console.WriteLine("\n=== Step 6: Current state after 5 seconds ===");
            Console.WriteLine($"Current URL: {Page.Url}");
            Console.WriteLine($"Page Title: {await Page.TitleAsync()}");
            await Page.ScreenshotAsync(new() { Path = "screenshots/03-after-login.png", FullPage = true });

            // Check if we're on customers page
            if (Page.Url.Contains("/customers"))
            {
                Console.WriteLine("\n✓ Successfully navigated to /customers page");

                // Check for h1
                try
                {
                    var h1 = await Page.Locator("h1").TextContentAsync();
                    Console.WriteLine($"Page Heading: {h1}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting h1: {ex.Message}");
                }

                // Check for table
                var tableCount = await Page.Locator("table").CountAsync();
                Console.WriteLine($"Table exists: {tableCount > 0}");

                if (tableCount > 0)
                {
                    var rowCount = await Page.Locator("tbody tr").CountAsync();
                    Console.WriteLine($"Number of rows in table: {rowCount}");

                    // Get first row data if exists
                    if (rowCount > 0)
                    {
                        var firstRowCells = await Page.Locator("tbody tr").First.Locator("td").AllAsync();
                        Console.WriteLine($"Number of cells in first row: {firstRowCells.Count}");
                        
                        for (int i = 0; i < Math.Min(firstRowCells.Count, 5); i++)
                        {
                            var cellText = await firstRowCells[i].TextContentAsync();
                            Console.WriteLine($"  Cell {i}: {cellText?.Trim()}");
                        }
                    }

                    // Check for New Customer button
                    var newCustomerBtnCount = await Page.Locator("button.btn-primary:has(i.bi-plus-lg)").CountAsync();
                    Console.WriteLine($"New Customer button exists: {newCustomerBtnCount > 0}");

                    // Check for total count in paginator
                    var paginatorTextCount = await Page.Locator(".pagination .active .page-link").CountAsync() > 0;
                    if (paginatorTextCount)
                    {
                        var totalCountText = await Page.Locator(".pagination .active .page-link").TextContentAsync();
                        Console.WriteLine($"Paginator text: {totalCountText?.Trim()}");
                    }
                }
                else
                {
                    Console.WriteLine("✗ Table not found on customers page!");
                    
                    // Check what's actually on the page
                    var bodyText = await Page.Locator("body").TextContentAsync();
                    var textLength = bodyText?.Length ?? 0;
                    Console.WriteLine($"Body text (first 500 chars): {bodyText?.Substring(0, Math.Min(500, textLength))}");
                }

                await Page.ScreenshotAsync(new() { Path = "screenshots/04-customers-page-final.png", FullPage = true });
            }
            else
            {
                Console.WriteLine($"\n✗ NOT on customers page. Current URL: {Page.Url}");

                // Check for error messages
                var errorCount = await Page.Locator(".alert-danger").CountAsync();
                if (errorCount > 0)
                {
                    var errorText = await Page.Locator(".alert-danger").TextContentAsync();
                    Console.WriteLine($"Error message: {errorText}");
                }

                // Check for any visible text
                var bodyText = await Page.Locator("body").TextContentAsync();
                var textLength = bodyText?.Length ?? 0;
                Console.WriteLine($"Body text (first 500 chars): {bodyText?.Substring(0, Math.Min(500, textLength))}");

                await Page.ScreenshotAsync(new() { Path = "screenshots/05-error-page.png", FullPage = true });
            }

            Console.WriteLine("\n=== Inspection Complete ===");
            
            // Keep browser open for manual inspection
            await Task.Delay(30000);
        }
    }
}
