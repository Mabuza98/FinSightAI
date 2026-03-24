import { useNavigate } from "react-router-dom";

function Settings() {
    const navigate = useNavigate();

    const user = JSON.parse(localStorage.getItem("user"));

    const handleLogout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        navigate("/");
    };

    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">Settings</h1>

            <div className="grid grid-cols-2 gap-6">

                {/* ACCOUNT */}
                <div className="glass p-6 rounded">
                    <h2 className="font-semibold mb-4">Account</h2>

                    <p className="text-sm text-gray-400">Name</p>
                    <p className="mb-3">{user?.name}</p>

                    <p className="text-sm text-gray-400">Email</p>
                    <p className="mb-4">{user?.email}</p>

                    <button
                        onClick={handleLogout}
                        className="bg-red-500 hover:bg-red-600 px-4 py-2 rounded"
                    >
                        Logout
                    </button>
                </div>

                {/* AI SETTINGS */}
                <div className="glass p-6 rounded">
                    <h2 className="font-semibold mb-4">AI Preferences</h2>

                    <label className="block mb-2 text-sm text-gray-400">
                        Response Style
                    </label>

                    <select className="w-full p-2 bg-black border rounded">
                        <option>Balanced</option>
                        <option>Concise</option>
                        <option>Detailed</option>
                    </select>

                    <p className="text-xs text-gray-500 mt-3">
                        Controls how AI responds to your queries.
                    </p>
                </div>

                {/* PLATFORM */}
                <div className="glass p-6 rounded">
                    <h2 className="font-semibold mb-4">Platform</h2>

                    <p className="text-sm text-gray-400">API Status</p>
                    <p className="text-green-400">● Online</p>

                    <p className="text-sm text-gray-400 mt-4">Version</p>
                    <p>v1.0 FinSight AI</p>
                </div>

                {/* SECURITY */}
                <div className="glass p-6 rounded">
                    <h2 className="font-semibold mb-4">Security</h2>

                    <p className="text-sm text-gray-400">Session</p>
                    <p className="text-green-400">Active</p>

                    <button
                        onClick={handleLogout}
                        className="mt-4 bg-red-500 hover:bg-red-600 px-4 py-2 rounded"
                    >
                        Logout All Devices
                    </button>
                </div>
            </div>
        </div>
    );
}

export default Settings;