import { Link } from "react-router-dom";

function Home() {
    return (
        <div className="space-y-8">
            {/* 🔥 HERO */}
            <div className="glass p-8 rounded text-center">
                <h1 className="text-4xl font-bold mb-4">
                    🚀 FinSight AI Platform
                </h1>
                <p className="text-gray-300 max-w-2xl mx-auto">
                    Enterprise Document Intelligence Platform powered by AI.
                    Analyze financial documents, detect risks, uncover growth opportunities,
                    and interact with your data using intelligent AI chat.
                </p>

                <div className="mt-6 flex justify-center gap-4">
                    <Link
                        to="/dashboard"
                        className="bg-green-500 px-6 py-3 rounded font-semibold hover:bg-green-600"
                    >
                        Go to Dashboard
                    </Link>

                    <Link
                        to="/chat"
                        className="bg-white/10 px-6 py-3 rounded hover:bg-white/20"
                    >
                        Start AI Chat
                    </Link>
                </div>
            </div>

            {/* 🔥 FEATURES */}
            <div className="grid grid-cols-3 gap-6">
                <div className="glass p-6 rounded">
                    <h2 className="text-xl font-semibold mb-2">📄 Document Intelligence</h2>
                    <p className="text-sm text-gray-300">
                        Upload financial reports, PDFs, and business documents.
                        Automatically process, chunk, and embed them using AI.
                    </p>
                </div>

                <div className="glass p-6 rounded">
                    <h2 className="text-xl font-semibold mb-2">🧠 AI Insights</h2>
                    <p className="text-sm text-gray-300">
                        Detect risks, growth signals, and opportunities using AI-driven analysis.
                    </p>
                </div>

                <div className="glass p-6 rounded">
                    <h2 className="text-xl font-semibold mb-2">💬 Smart Chat</h2>
                    <p className="text-sm text-gray-300">
                        Ask questions about your documents and get instant AI-powered answers.
                    </p>
                </div>
            </div>

            {/* 🔥 HOW IT WORKS */}
            <div className="glass p-6 rounded">
                <h2 className="text-2xl font-semibold mb-4">⚙️ How It Works</h2>

                <div className="grid grid-cols-4 gap-4 text-sm text-gray-300">
                    <div>
                        <span className="text-green-400 font-bold">1.</span> Upload Documents
                    </div>
                    <div>
                        <span className="text-green-400 font-bold">2.</span> AI Processing
                    </div>
                    <div>
                        <span className="text-green-400 font-bold">3.</span> Insights Detection
                    </div>
                    <div>
                        <span className="text-green-400 font-bold">4.</span> Query via Chat
                    </div>
                </div>
            </div>

            {/* 🔥 CTA */}
            <div className="glass p-6 rounded text-center">
                <h2 className="text-xl font-semibold mb-2">
                    Ready to analyze your documents?
                </h2>
                <Link
                    to="/documents"
                    className="bg-blue-500 px-6 py-3 rounded inline-block mt-3 hover:bg-blue-300"
                >
                    Upload Documents
                </Link>
            </div>
        </div>
    );
}

export default Home;