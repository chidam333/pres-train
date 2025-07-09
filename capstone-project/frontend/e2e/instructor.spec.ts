import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:4200/auth');
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('uma@gmail.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('uma#123');
  await page.getByRole('textbox', { name: 'Password' }).press('Tab');
  await page.getByRole('button', { name: 'Login →' }).click();
  await expect(page.locator('app-instructor-top-bar')).toContainText('Courses');
  await expect(page.locator('app-profile')).toContainText('instructor');
  await expect(page.locator('app-course-body')).toContainText('Angular: The documentry');
  await page.getByRole('link', { name: 'manage' }).first().click();
  await page.getByRole('button', { name: 'curriculum' }).click();
  await page.getByRole('button', { name: 'add lesson' }).click();
  await page.getByRole('textbox', { name: 'Title Material Title Material' }).fill('testing adding a lesson');
  await page.getByRole('textbox', { name: 'Description' }).click();
  await page.getByRole('textbox', { name: 'Description' }).fill('lesson adding getting tested');
  page.once('dialog', dialog => {
    console.log(`Dialog message: ${dialog.message()}`);
    dialog.accept().catch(() => {});
    // dialog.dismiss().catch(() => {});
  });
  await page.getByRole('button', { name: 'Save' }).click();
  page.once('dialog', dialog => {
    console.log(`Dialog message: ${dialog.message()}`);
    dialog.dismiss().catch(() => {});
  });
  await page.locator('form').filter({ hasText: 'Lesson 4 : edit Material' }).getByRole('button').first().click();
  page.once('dialog', dialog => {
    console.log(`Dialog message: ${dialog.message()}`);
    dialog.dismiss().catch(() => {});
  });
  await page.locator('form').filter({ hasText: 'Lesson 4 : edit Material' }).getByRole('button').first().click();
  await page.goto('http://localhost:4200/instructor/manage/2');
});