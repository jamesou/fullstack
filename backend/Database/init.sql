-- Create warehouse schema if it doesn't exist
CREATE SCHEMA IF NOT EXISTS warehouse;

-- Drop existing products table if it exists
DROP TABLE IF EXISTS warehouse.products;
DROP TABLE IF EXISTS products;

-- Create Products table
CREATE TABLE warehouse.products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    sku VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    price DECIMAL(10,2) NOT NULL,
    current_stock INTEGER NOT NULL DEFAULT 0,
    minimum_stock_level INTEGER NOT NULL DEFAULT 0,
    last_updated TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- Create index for frequently used columns
CREATE INDEX idx_products_sku ON warehouse.products(sku);
CREATE INDEX idx_products_name ON warehouse.products(name);
CREATE INDEX idx_products_is_active ON warehouse.products(is_active);

-- Insert sample data
INSERT INTO warehouse.products (name, sku, description, price, current_stock, minimum_stock_level)
VALUES 
    ('iPhone 14', 'IP14-128-BLK', 'iPhone 14 128GB Black', 999.99, 50, 10),
    ('MacBook Pro', 'MBP-14-M2', 'MacBook Pro 14" M2', 1999.99, 25, 5),
    ('AirPods Pro', 'APP-2-WHT', 'AirPods Pro 2nd Generation', 249.99, 100, 20),
    ('iPad Air', 'IPA-64-GRY', 'iPad Air 64GB Space Gray', 599.99, 30, 8),
    ('Apple Watch', 'AW-S8-45', 'Apple Watch Series 8 45mm', 399.99, 40, 10);
