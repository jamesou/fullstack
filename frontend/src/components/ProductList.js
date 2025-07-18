import React, { useState, useEffect } from 'react';
import { Table, Button, Input, Select, Space, message, Modal } from 'antd';
import axios from 'axios';
import ProductForm from './ProductForm';

const { Option } = Select;

const ProductList = ({ searchResults, lowStock }) => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [filters, setFilters] = useState({
        name: '',
        sku: '',
        minStock: '',
        maxStock: '',
        isActive: '',
        sortBy: '',
        sortOrder: ''
    });

    const fetchProducts = async () => {
        try {
            setLoading(true);
            const params = {
                ...Object.fromEntries(
                    Object.entries(filters).filter(([_, v]) => v !== '')
                ),
                lowStock: lowStock || undefined
            };
            const response = await axios.get('http://localhost:5039/api/products', { params });
            setProducts(response.data);
        } catch (error) {
            message.error('Failed to fetch products');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (searchResults !== null) {
            setProducts(searchResults);
        }
    }, [searchResults]);

    // Only fetch products when component mounts
    useEffect(() => {
        fetchProducts();
    }, []);

    const columns = [
        {
            title: 'Name',
            dataIndex: 'name',
            sorter: true,
        },
        {
            title: 'SKU',
            dataIndex: 'sku',
        },
        {
            title: 'Description',
            dataIndex: 'description',
        },
        {
            title: 'Price',
            dataIndex: 'price',
            render: (price) => `$${price.toFixed(2)}`,
        },
        {
            title: 'Current Stock',
            dataIndex: 'currentStock',
            sorter: true,
        },
        {
            title: 'Minimum Stock Level',
            dataIndex: 'minimumStockLevel',
        },
        {
            title: 'Status',
            dataIndex: 'isActive',
            render: (isActive) => (
                <span style={{ color: isActive ? 'green' : 'red' }}>
                    {isActive ? 'Active' : 'Inactive'}
                </span>
            ),
        },
        {
            title: 'Actions',
            key: 'actions',
            render: (_, record) => (
                <Space>
                    <Button onClick={() => handleEdit(record)}>Edit</Button>
                    <Button onClick={() => handleDelete(record.id)}>Delete</Button>
                    <Button onClick={() => handleAddStock(record)}>Add Stock</Button>
                    <Button onClick={() => handleRemoveStock(record)}>Remove Stock</Button>
                </Space>
            ),
        },
    ];

    const handleEdit = (product) => {
        // TODO: Implement edit functionality
        console.log('Edit product:', product);
    };

    const handleDelete = async (id) => {
        try {
            await axios.delete(`http://localhost:5039/api/products/${id}`);
            message.success('Product deleted successfully');
            fetchProducts();
        } catch (error) {
            message.error('Failed to delete product');
        }
    };

    const handleAddStock = async (product) => {
        // TODO: Implement add stock functionality
        console.log('Add stock to product:', product);
    };

    const handleRemoveStock = async (product) => {
        // TODO: Implement remove stock functionality
        console.log('Remove stock from product:', product);
    };

    return (
        <div style={{ padding: '20px' }}>
            <Space style={{ marginBottom: '16px', width: '100%', justifyContent: 'space-between' }}>
                <h1>Product Inventory Management</h1>
                <Button type="primary" onClick={() => setIsModalVisible(true)} icon={<span>+</span>}>
                    Add Product
                </Button>
            </Space>
            <Space style={{ marginBottom: '16px' }}>
                <Input
                    placeholder="Search by name"
                    value={filters.name}
                    onChange={(e) => setFilters({ ...filters, name: e.target.value })}
                />
                <Input
                    placeholder="Search by SKU"
                    value={filters.sku}
                    onChange={(e) => setFilters({ ...filters, sku: e.target.value })}
                />
                <Select
                    placeholder="Status"
                    allowClear
                    onChange={(value) => setFilters({ ...filters, isActive: value })}
                >
                    <Option value="true">Active</Option>
                    <Option value="false">Inactive</Option>
                </Select>
                <Button type="primary" onClick={fetchProducts} loading={loading}>
                    Search
                </Button>
                <Button onClick={() => {
                    setFilters({
                        name: '',
                        sku: '',
                        minStock: '',
                        maxStock: '',
                        isActive: '',
                        sortBy: '',
                        sortOrder: ''
                    });
                }}>
                    Clear
                </Button>
            </Space>
            <Table
                columns={columns}
                dataSource={products}
                loading={loading}
                rowKey="id"
            />
            <Modal
                title="Add New Product"
                open={isModalVisible}
                onCancel={() => setIsModalVisible(false)}
                footer={null}
                destroyOnClose
            >
                <ProductForm
                    onSuccess={() => {
                        setIsModalVisible(false);
                        fetchProducts();
                    }}
                />
            </Modal>
        </div>
    );
};

export default ProductList;
