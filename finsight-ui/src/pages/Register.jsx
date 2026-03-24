import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import API from "../api/axios";

function Register() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        name: "",
        email: "",
        password: "",
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();

        try {
            await API.post("/Auth/register", {
                id: 0,
                name: form.name,
                email: form.email,
                passwordHash: form.password,
            });

            alert("Registered successfully ✅");

            // redirect to login
            navigate("/");

        } catch (err) {
            console.error(err);
            alert("Registration failed ❌");
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen">
            <form
                onSubmit={handleRegister}
                className="glass p-8 rounded w-96"
            >
                <h2 className="text-2xl font-bold mb-6 text-center">
                    Create Account
                </h2>

                <input
                    name="name"
                    placeholder="Full Name"
                    className="w-full p-2 mb-3 bg-black border rounded"
                    onChange={handleChange}
                    required
                />

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
                    Register
                </button>

                <p className="text-sm mt-4 text-center">
                    Already have an account?{" "}
                    <Link to="/" className="text-green-400">
                        Login
                    </Link>
                </p>
            </form>
        </div>
    );
}

export default Register;