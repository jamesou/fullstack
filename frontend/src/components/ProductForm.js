import React, { useState } from 'react';
import { Form, Input, InputNumber, Switch, Button, message } from 'antd';
import axios from 'axios';

const ProductForm = ({ product, onSuccess }) => {
    const [loading, setLoading] = useState(false);

    const onFinish = async (values) => {
        try {
            setLoading(true);
            if (product) {
                // Update existing product
                await axios.put(`http://localhost:5039/api/products/${product.id}`, values);
                message.success('Product updated successfully');
            } else {
                // Create new product
                await axios.post('http://localhost:5039/api/products', values);
                message.success('Product created successfully');
            }
            onSuccess();
        } catch (error) {
            message.error(error.response?.data?.message || 'Operation failed');
        } finally {
            setLoading(false);
        }
    };

    return (
        <Form
            layout="vertical"
            initialValues={product || { isActive: true }}
            onFinish={onFinish}
        >
            <Form.Item
                name="name"
                label="Product Name"
                rules={[{ required: true, message: 'Please enter product name' }]}
            >
                <Input />
            </Form.Item>

            <Form.Item
                name="sku"
                label="SKU"
                rules={[{ required: true, message: 'Please enter SKU' }]}
            >
                <Input />
            </Form.Item>

            <Form.Item
                name="description"
                label="Description"
            >
                <Input.TextArea />
            </Form.Item>

            <Form.Item
                name="price"
                label="Price"
                rules={[{ required: true, message: 'Please enter price' }]}
            >
                <InputNumber
                    min={0}
                    precision={2}
                    style={{ width: '100%' }}
                />
            </Form.Item>

            <Form.Item
                name="minimumStockLevel"
                label="Minimum Stock Level"
                rules={[{ required: true, message: 'Please enter minimum stock level' }]}
            >
                <InputNumber min={0} style={{ width: '100%' }} />
            </Form.Item>

            <Form.Item
                name="currentStock"
                label="Initial Stock"
                rules={[{ required: false }]}
            >
                <InputNumber min={0} style={{ width: '100%' }} />
            </Form.Item>

            <Form.Item
                name="isActive"
                label="Active"
                valuePropName="checked"
            >
                <Switch />
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="submit" loading={loading} block>
                    {product ? 'Update Product' : 'Create Product'}
                </Button>
            </Form.Item>
        </Form>
    );
};

export default ProductForm;
