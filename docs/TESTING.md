# Verification plan

## 1. Automated build

The GitHub Actions Windows workflow compiles the existing Visual Studio solution. A successful build verifies that sources and references compile on that runner. It does **not** certify database behavior or security.

## 2. Database setup — currently blocked

- [ ] Obtain and inspect the original database schema
- [ ] Verify table names, foreign keys and column types used by each form
- [ ] Create a sanitized sample database with no real users or company data
- [ ] Use Windows authentication or another approved least-privilege connection method
- [ ] Run the app from a clean Windows environment

## 3. Manual functional checks — not yet completed

- [ ] App launches to the intended startup screen
- [ ] Manufacturer and customer login paths handle valid and invalid inputs safely
- [ ] Factories and warehouses load the expected records
- [ ] Product selection resolves the correct product and factory
- [ ] Order submission respects available stock and validates quantities
- [ ] Inventory updates are atomic and prevent invalid negative stock
- [ ] Errors do not leave SQL connections open or expose internal details
- [ ] Invalid/empty control selections do not crash the UI

## 4. Security blockers

The current legacy application compares entered passwords directly to database columns. Do not treat it as secure authentication. A production-grade rewrite would require a reviewed data model, salted password hashing, error handling, transactions, and authorization controls.

## Release decision

No production deployment or customer-data use until the database, security and workflow checks are completed with evidence.
