import { test, expect } from '@playwright/test';
import { environment } from '../environments/environment';

test('Url is in navbar', async ({ page }) => {
  await page.goto(environment.angularUrl);
  await page.locator('#auth').getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('emilyb@gmail.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('Password1234$');
  await page.getByRole('button', { name: 'Login' }).click();
  await expect(page.getByRole('heading', { name: 'Dashboard' })).toBeVisible();
  await expect(page.getByRole('link', { name: 'DMS' })).toBeVisible();
});

test('Title is in DMS', async ({ page }) => {
  await page.goto(environment.angularUrl);
  await page.locator('#auth').getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('emilyb@gmail.com');
  await page.getByRole('textbox', { name: 'Password' }).click();
  await page.getByRole('textbox', { name: 'Password' }).fill('Password1234$');
  await page.getByRole('textbox', { name: 'Password' }).press('Tab');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'DMS' }).click();
  await expect(
    page.getByRole('heading', { name: 'Document Management System' })
  ).toBeVisible();
});
