import React, { useState } from 'react';
import { Form, InputNumber, Button, message } from 'antd';
import axios from 'axios';

const StockOperationForm = ({ productId, operation, onSuccess }) => {
    const [loading, setLoading] = useState(false);

    const onFinish = async (values) => {
        try {
            setLoading(true);
            const url = `http://localhost:5039/api/products/${productId}/${operation}-stock`;
            await axios.post(url, values);
            message.success(`Stock ${operation}ed successfully`);
            onSuccess();
        } catch (error) {
            message.error(error.response?.data?.message || `Failed to ${operation} stock`);
        } finally {
            setLoading(false);
        }
    };

    return (
        <Form layout="vertical" onFinish={onFinish}>
            <Form.Item
                name="quantity"
                label="Quantity"
                rules={[{ required: true, message: 'Please enter quantity' }]}
            >
                <InputNumber min={1} style={{ width: '100%' }} />
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="submit" loading={loading} block>
                    {operation === 'add' ? 'Add Stock' : 'Remove Stock'}
                </Button>
            </Form.Item>
        </Form>
    );
};

export default StockOperationForm;
