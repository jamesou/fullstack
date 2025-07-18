import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import { Layout, Menu } from 'antd';
import ProductList from './components/ProductList';

const { Header, Content } = Layout;

function App() {
  return (
    <Router>
      <Layout>
        <Header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '0 24px' }}>
          <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['1']} style={{ flex: 1 }}>
            <Menu.Item key="1">
              <Link to="/">Products</Link>
            </Menu.Item>
            <Menu.Item key="2">
              <Link to="/low-stock">Low Stock</Link>
            </Menu.Item>
          </Menu>
        </Header>
        <Content style={{ padding: '20px', minHeight: 'calc(100vh - 64px)' }}>
          <Routes>
            <Route path="/" element={<ProductList />} />
            <Route path="/low-stock" element={<ProductList lowStock />} />
          </Routes>
        </Content>
      </Layout>
    </Router>
  );
}

export default App;
