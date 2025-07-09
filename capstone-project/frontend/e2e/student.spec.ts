import { expect, test } from '@playwright/test';

test('student workflow', async ({ page }) => {
  await page.goto('http://localhost:4200/auth');
  await page.getByRole('textbox', { name: 'Email' }).dblclick();
  await page
    .getByRole('textbox', { name: 'Email' })
    .fill('chidam3chain@gmail.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('chidam#123');
  await page.getByRole('textbox', { name: 'Password' }).press('Tab');
  await page.getByRole('button', { name: 'Login →' }).click();
  await expect(page.locator('app-home')).toContainText('Welcome back, chidam');
  if (
    !(await page
      .getByRole('link', { name: 'Angular: The documentry' })
      .isVisible())
  ) {
    await page.getByRole('button', { name: 'enroll', exact: true }).click();
  }
  await page.getByRole('link', { name: 'Angular: The documentry' }).click();
  await expect(page.getByRole('navigation')).toContainText(
    'Angular: The documentry'
  );
  await expect(page.getByRole('navigation')).toContainText(
    'Angular: The documentry'
  );
  await page.getByRole('heading', { name: 'uma' }).click();
  await expect(page.locator('h4')).toContainText('uma');
  await expect(page.locator('app-course-page-left')).toContainText(
    'uma@gmail.com'
  );
  await expect(page.locator('app-course-page-left')).toContainText(
    '6/22/25, 12:43 AM'
  );
});
