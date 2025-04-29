import { test, expect } from '@playwright/test';
import { environment } from '../environments/environment';

test('View users', async ({ page }) => {
  await page.goto(environment.angularUrl);
  await page.locator('#auth').getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page
    .getByRole('textbox', { name: 'Email' })
    .fill('admin@gmail.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('Password1234*');
  await page.getByRole('textbox', { name: 'Password' }).press('Tab');
  await page.getByRole('button', { name: 'Login' }).press('Enter');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Manage Users' }).click();
  await expect(
    page.getByRole('cell', { name: 'azcooldude8@gmail.com' })
  ).toBeVisible();
});
