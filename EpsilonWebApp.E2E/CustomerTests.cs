using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace EpsilonWebApp.E2E
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class CustomerTests : PageTest
    {
        private const string BaseUrl = "http://localhost:5234";

        [Test]
        public async Task Should_Login_And_Display_Customers()
        {
            await Page.GotoAsync($"{BaseUrl}/login");
            await Page.FillAsync("#username", "admin");
            await Page.FillAsync("#password", "password123");
            
            // Wait for navigation after click
            await Task.WhenAll(
                Page.WaitForURLAsync(new System.Text.RegularExpressions.Regex(".*/customers")),
                Page.ClickAsync("button[type='submit']")
            );

            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/customers"));
            await Expect(Page.Locator("h1")).ToBeVisibleAsync();
        }

        [Test]
        public async Task Should_Search_Address_And_AutoPopulate_Fields()
        {
            await LoginAsync();

            // 1. Click New Customer button
            await Page.ClickAsync("button.btn-primary:has(i.bi-plus-lg)");

            // 2. Wait for modal
            await Expect(Page.Locator(".modal-content")).ToBeVisibleAsync();

            // 3. Type in address field
            await Page.FillAsync("input[placeholder='Ξεκινήστε να πληκτρολογείτε τη διεύθυνση...']", "Athens");

            // 4. Wait for suggestions
            var suggestion = Page.Locator(".list-group-item").First;
            await Expect(suggestion).ToBeVisibleAsync(new() { Timeout = 15000 });

            // 5. Click suggestion
            await suggestion.ClickAsync();

            // 6. Verify populated fields by index
            var cityInput = Page.Locator(".modal-body .form-control").Nth(3);
            var countryInput = Page.Locator(".modal-body .form-control").Nth(6);

            await Expect(cityInput).Not.ToHaveValueAsync("");
            await Expect(countryInput).Not.ToHaveValueAsync("");
        }

        [Test]
        public async Task Should_Create_And_Delete_Customer()
        {
            await LoginAsync();

            string testCompanyName = "E2E Test " + Guid.NewGuid().ToString().Substring(0, 8);

            // Create
            await Page.ClickAsync("button.btn-primary:has(i.bi-plus-lg)");
            await Expect(Page.Locator(".modal-content")).ToBeVisibleAsync();
            
            await Page.Locator(".modal-body .form-control").Nth(0).FillAsync(testCompanyName);
            await Page.Locator(".modal-body .form-control").Nth(1).FillAsync("John E2E");
            await Page.Locator(".modal-body .form-control").Nth(7).FillAsync("1234567890");
            
            await Page.ClickAsync(".modal-footer button.btn-primary");

            // Wait for modal to disappear
            await Expect(Page.Locator(".modal-content")).Not.ToBeVisibleAsync();

            // Verify in table
            await Expect(Page.Locator("table")).ToContainTextAsync(testCompanyName);

            // Delete
            var row = Page.Locator("tr", new() { HasText = testCompanyName });
            await row.Locator("i.bi-trash3").ClickAsync();

            // Verify gone
            await Expect(Page.Locator("table")).Not.ToContainTextAsync(testCompanyName);
        }

        private async Task LoginAsync()
        {
            await Page.GotoAsync($"{BaseUrl}/login");
            await Page.FillAsync("#username", "admin");
            await Page.FillAsync("#password", "password123");
            
            await Task.WhenAll(
                Page.WaitForURLAsync(new System.Text.RegularExpressions.Regex(".*/customers")),
                Page.ClickAsync("button[type='submit']")
            );

            // Wait for either the table or the empty state message to appear
            await Page.WaitForSelectorAsync("table, .bi-people", new() { State = WaitForSelectorState.Visible });
        }
    }
}
