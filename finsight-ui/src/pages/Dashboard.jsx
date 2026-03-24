import { useEffect, useState } from "react";
import API from "../api/axios";
import {
    PieChart,
    Pie,
    Cell,
    Tooltip,
    ResponsiveContainer,
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    LineChart,
    Line,
} from "recharts";

function Dashboard() {
    const [insights, setInsights] = useState([]);
    const [documents, setDocuments] = useState([]);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const insightsRes = await API.get(
                "/Insights?query=financial performance risks growth opportunities insights"
            );
            const docsRes = await API.get("/Document/my-documents");

            setInsights(insightsRes.data);
            setDocuments(docsRes.data);
        } catch (err) {
            console.error(err);
        }
    };

    // 🔥 Counts
    const riskCount = insights.filter(i => i.type === "Risk").length;
    const growthCount = insights.filter(i => i.type === "Growth").length;
    const opportunityCount = insights.filter(i => i.type === "Opportunity").length;

    // 🔥 Pie Data
    const chartData = [
        { name: "Risk", value: riskCount },
        { name: "Growth", value: growthCount },
        { name: "Opportunity", value: opportunityCount },
    ];

    // 🔥 Bar Data
    const barData = [
        { name: "Risk", value: riskCount },
        { name: "Growth", value: growthCount },
        { name: "Opportunity", value: opportunityCount },
    ];

    // 🔥 Fake Trend Data (for now — later we make real)
    const trendData = [
        { month: "Jan", value: 20 },
        { month: "Feb", value: 40 },
        { month: "Mar", value: 35 },
        { month: "Apr", value: 60 },
        { month: "May", value: 55 },
    ];

    // 🔥 AI Health Score
    const total = riskCount + growthCount + opportunityCount;
    const healthScore = total === 0 ? 0 : Math.round((growthCount / total) * 100);

    const COLORS = ["#ef4444", "#22c55e", "#3b82f6"];

    return (
        <div>
            {/* HEADER */}
            <h1 className="text-3xl font-bold mb-6 bg-gradient-to-r from-green-400 to-blue-500 bg-clip-text text-transparent">
                Intelligence Dashboard
            </h1>

            {/* KPI CARDS */}
            <div className="grid grid-cols-4 gap-4 mb-8">
                <div className="glass p-4 rounded hover:scale-105 transition">
                    <h2 className="text-sm">Documents</h2>
                    <p className="text-2xl font-bold">{documents.length}</p>
                </div>

                <div className="glass p-4 rounded hover:scale-105 transition">
                    <h2 className="text-sm">Risks</h2>
                    <p className="text-2xl font-bold text-red-400">{riskCount}</p>
                </div>

                <div className="glass p-4 rounded hover:scale-105 transition">
                    <h2 className="text-sm">Growth</h2>
                    <p className="text-2xl font-bold text-green-400">{growthCount}</p>
                </div>

                <div className="glass p-4 rounded hover:scale-105 transition">
                    <h2 className="text-sm">Opportunities</h2>
                    <p className="text-2xl font-bold text-blue-400">{opportunityCount}</p>
                </div>
            </div>

            {/* HEALTH SCORE */}
            <div className="glass p-6 rounded mb-8">
                <h2 className="mb-2 font-semibold">Portfolio Health</h2>
                <div className="w-full bg-gray-700 rounded h-4">
                    <div
                        className="bg-green-400 h-4 rounded"
                        style={{ width: `${healthScore}%` }}
                    ></div>
                </div>
                <p className="mt-2 text-sm">{healthScore}% Healthy</p>
            </div>

            {/* CHARTS */}
            <div className="grid grid-cols-2 gap-6 mb-8">
                {/* PIE */}
                <div className="glass p-6 rounded">
                    <h2 className="mb-4 font-semibold">Insights Distribution</h2>
                    <ResponsiveContainer width="100%" height={250}>
                        <PieChart>
                            <Pie data={chartData} dataKey="value" outerRadius={80}>
                                {chartData.map((entry, index) => (
                                    <Cell key={index} fill={COLORS[index]} />
                                ))}
                            </Pie>
                            <Tooltip />
                        </PieChart>
                    </ResponsiveContainer>
                </div>

                {/* BAR */}
                <div className="glass p-6 rounded">
                    <h2 className="mb-4 font-semibold">Category Comparison</h2>
                    <ResponsiveContainer width="100%" height={250}>
                        <BarChart data={barData}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="name" />
                            <YAxis />
                            <Tooltip />
                            <Bar dataKey="value" fill="#3b82f6" />
                        </BarChart>
                    </ResponsiveContainer>
                </div>
            </div>

            {/* TREND */}
            <div className="glass p-6 rounded mb-8">
                <h2 className="mb-4 font-semibold">Trend Analysis</h2>
                <ResponsiveContainer width="100%" height={250}>
                    <LineChart data={trendData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" />
                        <YAxis />
                        <Tooltip />
                        <Line type="monotone" dataKey="value" stroke="#22c55e" />
                    </LineChart>
                </ResponsiveContainer>
            </div>

            {/* INSIGHTS + DOCUMENTS */}
            <div className="grid grid-cols-2 gap-6">
                {/* INSIGHTS */}
                <div className="glass p-6 rounded">
                    <h2 className="mb-4 font-semibold">AI Insights</h2>
                    <div className="space-y-3 max-h-80 overflow-y-auto">
                        {insights.map((item, i) => (
                            <div
                                key={i}
                                className="bg-white/5 p-4 rounded border border-white/10 hover:bg-white/10 transition"
                            >
                                <h3 className="font-bold">{item.title}</h3>
                                <p className="text-sm">{item.description}</p>

                                <span
                                    className={`text-xs mt-2 inline-block px-2 py-1 rounded ${item.type === "Risk"
                                            ? "bg-red-500"
                                            : item.type === "Growth"
                                                ? "bg-green-500"
                                                : "bg-blue-500"
                                        }`}
                                >
                                    {item.type}
                                </span>
                            </div>
                        ))}
                    </div>
                </div>

                {/* DOCUMENTS */}
                <div className="glass p-6 rounded">
                    <h2 className="mb-4 font-semibold">Top Documents</h2>
                    <div className="space-y-3">
                        {documents.map((doc, i) => (
                            <div
                                key={i}
                                className="bg-white/5 p-3 rounded border border-white/10"
                            >
                                📄 {doc.fileName}
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* ACTIVITY */}
            <div className="glass p-6 rounded mt-8">
                <h2 className="mb-4 font-semibold">Recent Activity</h2>
                <ul className="text-sm space-y-2">
                    <li>📄 Document uploaded</li>
                    <li>🤖 AI query executed</li>
                    <li>📊 Insights generated</li>
                </ul>
            </div>
        </div>
    );
}

export default Dashboard;