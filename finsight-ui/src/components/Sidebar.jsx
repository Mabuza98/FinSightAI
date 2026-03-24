import { Link } from "react-router-dom";

function Sidebar() {
    return (
        <div className="w-64 bg-black text-white min-h-screen p-6">
            <h2 className="text-xl font-bold mb-8">FinSight AI</h2>

            <nav className="space-y-4">
                <Link to="/home">🏠 Home</Link>

                <Link to="/dashboard" className="block hover:text-green-400">
                    📊 Dashboard
                </Link>

                <Link to="/chat" className="block hover:text-green-400">
                    💬 AI Chat
                </Link>

                <Link to="/documents" className="block hover:text-green-400">
                    📄 Documents
                </Link>

                <Link to="/insights" className="block hover:text-green-400">
                    🧠 Insights
                </Link>

                <Link to="/Settings" className="block hover:text-green-400">
                    ⚙️ Settings
                </Link>
            </nav>
        </div>
    );
}

export default Sidebar;