# Playwright Selectors Report for EpsilonWebApp

**Generated:** February 15, 2026  
**Application URL:** http://localhost:5234  
**Test Credentials:** admin / password123

---

## Table of Contents
1. [Login Page Selectors](#login-page-selectors)
2. [Customers Page Selectors](#customers-page-selectors)
3. [Customer Modal Selectors](#customer-modal-selectors)
4. [Complete Test Examples](#complete-test-examples)

---

## Login Page Selectors

**URL:** `http://localhost:5234/login`

### Form Fields

| Element | Selector | Type | Notes |
|---------|----------|------|-------|
| Username Field | `#username` | ID | Most reliable, has `name="username"` |
| Password Field | `#password` | ID | Most reliable, has `name="password"` |
| Login Button | `button[type="submit"]` | Attribute | Contains Greek text "Είσοδος" |

### Alternative Selectors

```typescript
// Username alternatives
await page.fill('#username', 'admin');
await page.fill('input[name="username"]', 'admin');
await page.fill('input[type="text"]', 'admin');

// Password alternatives
await page.fill('#password', 'password123');
await page.fill('input[name="password"]', 'password123');
await page.fill('input[type="password"]', 'password123');

// Login button alternatives
await page.click('button[type="submit"]');
await page.click('button.btn-primary.w-100');
await page.getByRole('button', { name: /είσοδος/i }).click();
```

### Source Code Reference
```razor
<input type="text" class="form-control" id="username" name="username" required />
<input type="password" class="form-control" id="password" name="password" required />
<button type="submit" class="btn btn-primary w-100">Είσοδος</button>
```

---

## Customers Page Selectors

**URL:** `http://localhost:5234/customers`

### Main Elements

| Element | Selector | Type | Notes |
|---------|----------|------|-------|
| Page Heading | `h1` | Tag | Contains text "Πελάτες" |
| New Customer Button | `button.btn-primary:has(i.bi-plus-lg)` | CSS + Has | Icon-based selector (recommended) |
| Customers Table | `table.table-striped` | Class | Also has `table-hover` class |
| Table Header | `thead.table-secondary` | Class | Contains sortable column headers |
| Table Body | `tbody` | Tag | Contains customer rows |
| Table Rows | `tbody tr` | CSS | Each row is a customer |
| Table Cells | `tbody tr td` | CSS | 6 cells per row |
| Total Count | `.mb-2.text-muted strong` | CSS | Shows total customer count |
| Pagination | `.pagination` | Class | Navigation controls |

### New Customer Button - Multiple Options

```typescript
// Option 1: Icon-based (RECOMMENDED - avoids Greek text encoding)
await page.click('button.btn-primary:has(i.bi-plus-lg)');

// Option 2: Title attribute (Greek text)
await page.click('button[title="Νέος Πελάτης"]');

// Option 3: Parent container + class
await page.click('.d-flex.justify-content-between button.btn-primary');

// Option 4: Playwright's has selector
await page.locator('button.btn-primary').filter({ has: page.locator('i.bi-plus-lg') }).click();
```

### Table Selectors

```typescript
// Get all customer rows
const rows = await page.locator('tbody tr').all();

// Get specific row by company name
const row = page.locator('tr', { hasText: 'Company Name' });

// Get first row
const firstRow = page.locator('tbody tr').first();

// Get table cells from first row
const cells = await page.locator('tbody tr').first().locator('td').all();

// Table headers (sortable columns)
const headers = await page.locator('thead.table-secondary th').all();
// Headers: Επωνυμία Εταιρείας, Όνομα Επαφής, Πόλη, Χώρα, Τηλέφωνο, [Actions]

// Verify table contains text
await expect(page.locator('table')).toContainText('Expected Company Name');
```

### Action Buttons (Edit/Delete)

```typescript
// Edit button (pencil icon)
const editButton = page.locator('button:has(i.bi-pencil-square)');
await editButton.first().click();

// Delete button (trash icon)
const deleteButton = page.locator('button:has(i.bi-trash3)');
await deleteButton.first().click();

// Edit button in specific row
await page.locator('tr', { hasText: 'Company Name' })
  .locator('button:has(i.bi-pencil-square)')
  .click();

// Delete button in specific row
await page.locator('tr', { hasText: 'Company Name' })
  .locator('i.bi-trash3')
  .click();
```

### Pagination Selectors

```typescript
// Pagination container
const pagination = page.locator('.pagination');

// First page button (double chevron left)
await page.locator('button:has(i.bi-chevron-double-left)').click();

// Previous page button
await page.locator('button:has(i.bi-chevron-left)').click();

// Next page button
await page.locator('button:has(i.bi-chevron-right)').click();

// Last page button (double chevron right)
await page.locator('button:has(i.bi-chevron-double-right)').click();

// Current page indicator
const currentPage = page.locator('.page-item.active .page-link');
```

### Sorting

```typescript
// Click on column header to sort
// Headers are clickable and have cursor: pointer style
const companyHeader = page.locator('thead th').first(); // Επωνυμία Εταιρείας
await companyHeader.click(); // Sort ascending
await companyHeader.click(); // Sort descending

// Sort icons
// Unsorted: i.bi-arrow-down-up
// Ascending: i.bi-sort-up
// Descending: i.bi-sort-down
```

### Source Code Reference
```razor
<h1>Πελάτες</h1>
<button class="btn btn-primary" @onclick="CreateNewCustomer" title="Νέος Πελάτης">
    <i class="bi bi-plus-lg"></i>
</button>

<table class="table table-striped table-hover">
    <thead class="table-secondary">
        <tr>
            <th>Επωνυμία Εταιρείας</th>
            <th>Όνομα Επαφής</th>
            <th>Πόλη</th>
            <th>Χώρα</th>
            <th>Τηλέφωνο</th>
            <th class="text-center"></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>@customer.CompanyName</td>
            <td>@customer.ContactName</td>
            <td>@customer.City</td>
            <td>@customer.Country</td>
            <td>@customer.Phone</td>
            <td class="text-center">
                <button class="btn btn-sm text-primary p-0 me-3 border-0 bg-transparent" title="Επεξεργασία">
                    <i class="bi bi-pencil-square"></i>
                </button>
                <button class="btn btn-sm text-danger p-0 border-0 bg-transparent" title="Διαγραφή">
                    <i class="bi bi-trash3"></i>
                </button>
            </td>
        </tr>
    </tbody>
</table>
```

---

## Customer Modal Selectors

**Triggered by:** Clicking "New Customer" button or Edit button

### Modal Container

```typescript
// Wait for modal to appear
await page.waitForSelector('.modal-content', { state: 'visible' });
await expect(page.locator('.modal-content')).toBeVisible();

// Modal is shown with class: "modal fade show d-block"
const modal = page.locator('.modal-content');
```

### Modal Elements

| Element | Selector | Notes |
|---------|----------|-------|
| Modal Container | `.modal-content` | Main modal wrapper |
| Modal Header | `.modal-header` | Contains title and close button |
| Modal Title | `.modal-title` | "Νέος Πελάτης" or "Επεξεργασία Πελάτη" |
| Close Button (X) | `.btn-close` | Top-right close button |
| Modal Body | `.modal-body` | Contains the form |
| Modal Footer | `.modal-footer` | Contains Cancel and Save buttons |

### Form Fields (by Index)

**IMPORTANT:** Form fields should be accessed by index using `.nth()` to avoid Greek text encoding issues.

```typescript
// Access form fields by index (0-based)
const companyName = page.locator('.modal-body .form-control').nth(0);
const contactName = page.locator('.modal-body .form-control').nth(1);
const address = page.locator('.modal-body .form-control').nth(2); // Autocomplete input
const city = page.locator('.modal-body .form-control').nth(3);
const region = page.locator('.modal-body .form-control').nth(4);
const postalCode = page.locator('.modal-body .form-control').nth(5);
const country = page.locator('.modal-body .form-control').nth(6);
const phone = page.locator('.modal-body .form-control').nth(7);

// Fill the form
await companyName.fill('Test Company');
await contactName.fill('John Doe');
await phone.fill('1234567890');
```

### Form Fields Mapping

| Index | Field Name (English) | Field Name (Greek) | Notes |
|-------|---------------------|-------------------|-------|
| 0 | Company Name | Επωνυμία Εταιρείας | Required |
| 1 | Contact Name | Όνομα Επαφής | Required |
| 2 | Address | Διεύθυνση | Autocomplete input |
| 3 | City | Πόλη | Auto-populated from address |
| 4 | Region | Περιοχή | Auto-populated from address |
| 5 | Postal Code | Ταχυδρομικός Κώδικας | Auto-populated from address |
| 6 | Country | Χώρα | Auto-populated from address |
| 7 | Phone | Τηλέφωνο | Required |

### Address Autocomplete

```typescript
// Address input with placeholder
const addressInput = page.locator('input[placeholder*="πληκτρολογείτε"]');
// Full placeholder: "Ξεκινήστε να πληκτρολογείτε τη διεύθυνση..."

// Type to trigger autocomplete
await addressInput.fill('Athens');

// Wait for suggestions to appear
const suggestions = page.locator('.list-group-item');
await expect(suggestions.first()).toBeVisible();

// Click first suggestion
await suggestions.first().click();

// Verify fields are auto-populated
await expect(city).not.toHaveValue('');
await expect(country).not.toHaveValue('');
```

### Modal Buttons

```typescript
// Cancel button
await page.locator('.modal-footer button.btn-secondary').click();
await page.getByRole('button', { name: /ακύρωση/i }).click();

// Save button (submit)
await page.locator('.modal-footer button[type="submit"]').click();
await page.locator('button[type="submit"]').click();
await page.getByRole('button', { name: /αποθήκευση/i }).click();

// Close button (X)
await page.locator('.btn-close').click();
```

### Source Code Reference
```razor
<div class="modal fade show d-block" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Νέος Πελάτης / Επεξεργασία Πελάτη</h5>
                <button type="button" class="btn-close" @onclick="CloseModal"></button>
            </div>
            <div class="modal-body">
                <EditForm Model="editingCustomer" OnValidSubmit="HandleSubmit">
                    <!-- Form fields here -->
                    <InputText class="form-control" @bind-Value="editingCustomer.CompanyName" />
                    <InputText class="form-control" @bind-Value="editingCustomer.ContactName" />
                    <input type="text" class="form-control" 
                           placeholder="Ξεκινήστε να πληκτρολογείτε τη διεύθυνση..." />
                    <!-- ... more fields ... -->
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary">Ακύρωση</button>
                        <button type="submit" class="btn btn-primary">Αποθήκευση</button>
                    </div>
                </EditForm>
            </div>
        </div>
    </div>
</div>
```

---

## Complete Test Examples

### Example 1: Login Test

```typescript
import { test, expect } from '@playwright/test';

test('should login successfully', async ({ page }) => {
  // Navigate to login page
  await page.goto('http://localhost:5234/login');
  
  // Fill login form
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  
  // Click login button
  await page.click('button[type="submit"]');
  
  // Verify redirect to customers page
  await expect(page).toHaveURL('http://localhost:5234/customers');
  
  // Verify page heading
  await expect(page.locator('h1')).toHaveText('Πελάτες');
});
```

### Example 2: Create Customer Test

```typescript
test('should create a new customer', async ({ page }) => {
  // Login first
  await page.goto('http://localhost:5234/login');
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  await page.click('button[type="submit"]');
  await expect(page).toHaveURL('http://localhost:5234/customers');
  
  // Click New Customer button
  await page.click('button.btn-primary:has(i.bi-plus-lg)');
  
  // Wait for modal
  await expect(page.locator('.modal-content')).toBeVisible();
  
  // Fill form fields by index
  const testCompany = `Test Corp ${Date.now()}`;
  await page.locator('.modal-body .form-control').nth(0).fill(testCompany);
  await page.locator('.modal-body .form-control').nth(1).fill('John Doe');
  await page.locator('.modal-body .form-control').nth(7).fill('1234567890');
  
  // Submit form
  await page.click('button[type="submit"]');
  
  // Verify customer appears in table
  await expect(page.locator('table')).toContainText(testCompany);
});
```

### Example 3: Address Autocomplete Test

```typescript
test('should autocomplete address and populate fields', async ({ page }) => {
  // Login and open new customer modal
  await page.goto('http://localhost:5234/login');
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  await page.click('button[type="submit"]');
  await page.click('button.btn-primary:has(i.bi-plus-lg)');
  await expect(page.locator('.modal-content')).toBeVisible();
  
  // Type in address field
  const addressInput = page.locator('input[placeholder*="πληκτρολογείτε"]');
  await addressInput.fill('Athens');
  
  // Wait for suggestions
  const suggestions = page.locator('.list-group-item');
  await expect(suggestions.first()).toBeVisible();
  
  // Click first suggestion
  await suggestions.first().click();
  
  // Verify auto-populated fields
  const city = page.locator('.modal-body .form-control').nth(3);
  const country = page.locator('.modal-body .form-control').nth(6);
  
  await expect(city).not.toHaveValue('');
  await expect(country).not.toHaveValue('');
});
```

### Example 4: Edit and Delete Customer Test

```typescript
test('should edit and delete customer', async ({ page }) => {
  // Login
  await page.goto('http://localhost:5234/login');
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  await page.click('button[type="submit"]');
  
  // Create a test customer first
  await page.click('button.btn-primary:has(i.bi-plus-lg)');
  await expect(page.locator('.modal-content')).toBeVisible();
  
  const testCompany = `E2E Test ${Date.now()}`;
  await page.locator('.modal-body .form-control').nth(0).fill(testCompany);
  await page.locator('.modal-body .form-control').nth(1).fill('Test User');
  await page.locator('.modal-body .form-control').nth(7).fill('9999999999');
  await page.click('button[type="submit"]');
  
  // Wait for modal to close and table to update
  await expect(page.locator('.modal-content')).not.toBeVisible();
  await expect(page.locator('table')).toContainText(testCompany);
  
  // Find the row and click edit
  const row = page.locator('tr', { hasText: testCompany });
  await row.locator('button:has(i.bi-pencil-square)').click();
  
  // Wait for modal to open
  await expect(page.locator('.modal-content')).toBeVisible();
  
  // Edit the company name
  const updatedName = `${testCompany} Updated`;
  await page.locator('.modal-body .form-control').nth(0).clear();
  await page.locator('.modal-body .form-control').nth(0).fill(updatedName);
  await page.click('button[type="submit"]');
  
  // Verify updated name appears
  await expect(page.locator('table')).toContainText(updatedName);
  
  // Delete the customer
  const updatedRow = page.locator('tr', { hasText: updatedName });
  await updatedRow.locator('i.bi-trash3').click();
  
  // Verify customer is removed
  await expect(page.locator('table')).not.toContainText(updatedName);
});
```

### Example 5: Table Sorting Test

```typescript
test('should sort customers by column', async ({ page }) => {
  // Login
  await page.goto('http://localhost:5234/login');
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  await page.click('button[type="submit"]');
  
  // Get first company name before sorting
  const firstCellBefore = await page.locator('tbody tr').first().locator('td').first().textContent();
  
  // Click company name header to sort
  const companyHeader = page.locator('thead.table-secondary th').first();
  await companyHeader.click();
  
  // Wait for table to update
  await page.waitForTimeout(500);
  
  // Get first company name after sorting
  const firstCellAfter = await page.locator('tbody tr').first().locator('td').first().textContent();
  
  // Verify sorting changed the order (in most cases)
  // Note: This might be the same if data is already sorted
  console.log(`Before: ${firstCellBefore}, After: ${firstCellAfter}`);
  
  // Verify sort icon changed
  await expect(companyHeader.locator('i.bi-sort-up, i.bi-sort-down')).toBeVisible();
});
```

### Example 6: Pagination Test

```typescript
test('should navigate through pages', async ({ page }) => {
  // Login
  await page.goto('http://localhost:5234/login');
  await page.fill('#username', 'admin');
  await page.fill('#password', 'password123');
  await page.click('button[type="submit"]');
  
  // Check if pagination exists (only if more than 10 customers)
  const pagination = page.locator('.pagination');
  const isVisible = await pagination.isVisible();
  
  if (isVisible) {
    // Get current page text
    const currentPageBefore = await page.locator('.page-item.active .page-link').textContent();
    console.log(`Current page: ${currentPageBefore}`);
    
    // Click next page
    const nextButton = page.locator('button:has(i.bi-chevron-right)');
    const isEnabled = !(await nextButton.locator('..').getAttribute('class'))?.includes('disabled');
    
    if (isEnabled) {
      await nextButton.click();
      await page.waitForTimeout(500);
      
      // Verify page changed
      const currentPageAfter = await page.locator('.page-item.active .page-link').textContent();
      console.log(`Current page after: ${currentPageAfter}`);
      expect(currentPageAfter).not.toBe(currentPageBefore);
    }
  }
});
```

---

## Best Practices

### 1. Avoid Greek Text in Selectors
❌ **Bad:** `await page.click('button:has-text("Νέος Πελάτης")')`  
✅ **Good:** `await page.click('button.btn-primary:has(i.bi-plus-lg)')`

### 2. Use Icon-Based Selectors
Icons are language-independent and more reliable:
- New Customer: `button:has(i.bi-plus-lg)`
- Edit: `button:has(i.bi-pencil-square)`
- Delete: `button:has(i.bi-trash3)`

### 3. Use Index-Based Selection for Form Fields
When labels contain non-ASCII characters, use `.nth()`:
```typescript
await page.locator('.modal-body .form-control').nth(0).fill('Company');
```

### 4. Wait for Modal Visibility
Always wait for modals to be visible before interacting:
```typescript
await expect(page.locator('.modal-content')).toBeVisible();
```

### 5. Use ID Selectors When Available
IDs are the most reliable selectors:
- `#username`
- `#password`

### 6. Combine Selectors for Specificity
```typescript
// Find delete button in specific row
await page.locator('tr', { hasText: 'Company Name' })
  .locator('button:has(i.bi-trash3)')
  .click();
```

---

## Selector Priority Recommendations

1. **ID selectors** (`#username`) - Most reliable
2. **Icon-based selectors** (`button:has(i.bi-plus-lg)`) - Language-independent
3. **Attribute selectors** (`button[type="submit"]`) - Semantic
4. **Class selectors** (`.modal-content`) - Stable
5. **Index-based selectors** (`.nth(0)`) - For form fields with non-ASCII labels
6. **Text-based selectors** - Avoid for Greek text due to encoding issues

---

## Summary

This report provides comprehensive Playwright selectors for the EpsilonWebApp. All selectors have been verified against the actual source code and existing E2E tests. The selectors prioritize:

- **Reliability**: Using IDs and stable class names
- **Language Independence**: Using icon classes instead of Greek text
- **Maintainability**: Clear, semantic selectors that are easy to understand

For any questions or updates, refer to the source files:
- Login: `EpsilonWebApp/Components/Pages/Login.razor`
- Customers: `EpsilonWebApp.Client/Pages/Customers.razor`
- Existing Tests: `EpsilonWebApp.E2E/CustomerTests.cs`
