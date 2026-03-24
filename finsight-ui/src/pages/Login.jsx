import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import API from "../api/axios";

function Login() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        email: "",
        password: "",
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const res = await API.post("/Auth/login", form);

            // ✅ SAVE DATA
            localStorage.setItem("token", res.data.token);
            localStorage.setItem("user", JSON.stringify(res.data.user));

            navigate("/Home");

        } catch (err) {
            console.error(err);
            alert("Login failed ❌");
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen">
            <form
                onSubmit={handleLogin}
                className="glass p-8 rounded w-96"
            >
                <h2 className="text-2xl font-bold mb-6 text-center">
                    FinSight AI Login
                </h2>

                <input
                    name="email"
                    type="email"
                    placeholder="Email"
                    className="w-full p-2 mb-3 bg-black border rounded"
                    onChange={handleChange}
                    required
                />

                <input
                    name="password"
                    type="password"
                    placeholder="Password"
                    className="w-full p-2 mb-4 bg-black border rounded"
                    onChange={handleChange}
                    required
                />

                <button className="w-full bg-green-500 py-2 rounded">
                    Login
                </button>

                <p className="text-sm mt-4 text-center">
                    No account?{" "}
                    <Link to="/register" className="text-green-400">
                        Register
                    </Link>
                </p>
            </form>
        </div>
    );
}

export default Login;