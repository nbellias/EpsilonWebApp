# Playwright Selectors - Quick Reference

## Login Page (`/login`)

```typescript
// Form fields
await page.fill('#username', 'admin');
await page.fill('#password', 'password123');
await page.click('button[type="submit"]');
```

## Customers Page (`/customers`)

### Main Elements
```typescript
// Page heading
page.locator('h1')  // "Πελάτες"

// New Customer button (RECOMMENDED - icon-based)
page.locator('button.btn-primary:has(i.bi-plus-lg)')

// Table
page.locator('table.table-striped')
page.locator('tbody tr')  // All rows
page.locator('tbody tr').first()  // First row
page.locator('tbody tr td')  // All cells

// Total count
page.locator('.mb-2.text-muted strong')
```

### Action Buttons
```typescript
// Edit button (pencil icon)
page.locator('button:has(i.bi-pencil-square)')

// Delete button (trash icon)
page.locator('button:has(i.bi-trash3)')

// Action in specific row
page.locator('tr', { hasText: 'Company Name' })
  .locator('button:has(i.bi-pencil-square)')
```

### Pagination
```typescript
page.locator('.pagination')
page.locator('button:has(i.bi-chevron-double-left)')  // First
page.locator('button:has(i.bi-chevron-left)')         // Previous
page.locator('button:has(i.bi-chevron-right)')        // Next
page.locator('button:has(i.bi-chevron-double-right)') // Last
page.locator('.page-item.active .page-link')          // Current page
```

## Customer Modal (New/Edit)

### Modal Container
```typescript
// Wait for modal
await page.waitForSelector('.modal-content', { state: 'visible' });

// Modal elements
page.locator('.modal-content')
page.locator('.modal-title')
page.locator('.btn-close')  // X button
```

### Form Fields (by index - RECOMMENDED)
```typescript
// Access by index to avoid Greek text encoding issues
page.locator('.modal-body .form-control').nth(0)  // Company Name
page.locator('.modal-body .form-control').nth(1)  // Contact Name
page.locator('.modal-body .form-control').nth(2)  // Address (autocomplete)
page.locator('.modal-body .form-control').nth(3)  // City
page.locator('.modal-body .form-control').nth(4)  // Region
page.locator('.modal-body .form-control').nth(5)  // Postal Code
page.locator('.modal-body .form-control').nth(6)  // Country
page.locator('.modal-body .form-control').nth(7)  // Phone
```

### Address Autocomplete
```typescript
// Address input
page.locator('input[placeholder*="πληκτρολογείτε"]')

// Suggestions
page.locator('.list-group-item')
page.locator('.list-group-item').first()
```

### Modal Buttons
```typescript
// Cancel
page.locator('.modal-footer button.btn-secondary')

// Save (submit)
page.locator('button[type="submit"]')

// Close (X)
page.locator('.btn-close')
```

## Complete Login Flow
```typescript
await page.goto('http://localhost:5234/login');
await page.fill('#username', 'admin');
await page.fill('#password', 'password123');
await page.click('button[type="submit"]');
await expect(page).toHaveURL('http://localhost:5234/customers');
```

## Complete Create Customer Flow
```typescript
// Click New Customer
await page.click('button.btn-primary:has(i.bi-plus-lg)');
await expect(page.locator('.modal-content')).toBeVisible();

// Fill form
await page.locator('.modal-body .form-control').nth(0).fill('Test Company');
await page.locator('.modal-body .form-control').nth(1).fill('John Doe');
await page.locator('.modal-body .form-control').nth(7).fill('1234567890');

// Submit
await page.click('button[type="submit"]');

// Verify
await expect(page.locator('table')).toContainText('Test Company');
```

## Best Practices

✅ **DO:**
- Use ID selectors when available (`#username`, `#password`)
- Use icon-based selectors (`button:has(i.bi-plus-lg)`)
- Use index-based selection for form fields (`.nth(0)`)
- Wait for modal visibility before interacting

❌ **DON'T:**
- Use Greek text in selectors (encoding issues)
- Use text-based selectors for buttons with Greek labels
- Interact with modals before checking visibility
