import { BrowserRouter, Routes, Route } from "react-router-dom";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import Chat from "./pages/Chat";
import Documents from "./pages/Documents";
import Insights from "./pages/Insights";
import InsightDetail from "./pages/InsightDetail";
import Settings from "./pages/Settings";
import Layout from "./components/Layout";
import ProtectedRoute from "./components/ProtectedRoute";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                {/* PUBLIC */}
                <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* PROTECTED + LAYOUT */}
                <Route
                    path="/"
                    element={
                        <ProtectedRoute>
                            <Layout />
                        </ProtectedRoute>
                    }
                >
                    <Route path="home" element={<Home />} />
                    <Route path="dashboard" element={<Dashboard />} />
                    <Route path="chat" element={<Chat />} />
                    <Route path="documents" element={<Documents />} />
                    <Route path="insights" element={<Insights />} />
                    <Route path="insightS/:id" element={<InsightDetail />} />
                    <Route path="settings" element={<Settings />} />
                    <Route path="/" element={<Login />} />
                    
                    
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;