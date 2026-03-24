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
} from "recharts";

function Documents() {
    const [docs, setDocs] = useState([]);
    const [file, setFile] = useState(null);
    const [loading, setLoading] = useState(false);
    const [selectedDoc, setSelectedDoc] = useState(null);
    const [insights, setInsights] = useState([]);
    const [insightLoading, setInsightLoading] = useState(false);
    const [search, setSearch] = useState("");
    const [activeTab, setActiveTab] = useState("All");

    const loadDocs = async () => {
        try {
            const res = await API.get("/Document/my-documents");
            setDocs(res.data);
        } catch (err) {
            console.error(err);
        }
    };

    useEffect(() => {
        loadDocs();
    }, []);

    const uploadFile = async () => {
        if (!file) return alert("Select a file");

        const formData = new FormData();
        formData.append("file", file);

        setLoading(true);

        try {
            await API.post("/Document/upload-single", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });

            setFile(null);
            loadDocs();
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const loadInsights = async (fileName) => {
        setSelectedDoc(fileName);
        setInsightLoading(true);

        try {
            const res = await API.get(
                `/Insights?query=${fileName} financial risks growth opportunities`
            );
            setInsights(res.data);
        } catch (err) {
            console.error(err);
        } finally {
            setInsightLoading(false);
        }
    };

    const filteredDocs = docs.filter((doc) =>
        doc.fileName.toLowerCase().includes(search.toLowerCase())
    );

    // 🔥 COUNTS
    const riskCount = insights.filter(i => i.type === "Risk").length;
    const growthCount = insights.filter(i => i.type === "Growth").length;
    const opportunityCount = insights.filter(i => i.type === "Opportunity").length;

    // 🔥 RISK SCORE (simple AI-style metric)
    const total = riskCount + growthCount + opportunityCount;
    const riskScore = total === 0 ? 0 : Math.round((riskCount / total) * 100);

    // 🔥 CHART DATA
    const chartData = [
        { name: "Risk", value: riskCount },
        { name: "Growth", value: growthCount },
        { name: "Opportunity", value: opportunityCount },
    ];

    const COLORS = ["#ef4444", "#22c55e", "#3b82f6"];

    // 🔥 FILTER INSIGHTS BY TAB
    const filteredInsights =
        activeTab === "All"
            ? insights
            : insights.filter(i => i.type === activeTab);

    return (
        <div>
            <h1 className="text-2xl font-bold mb-6">📄 Documents Intelligence</h1>

            {/* Upload */}
            <div className="glass p-4 mb-6 rounded">
                <h2 className="mb-2">Upload Document</h2>

                <div className="flex items-center gap-3">
                    <input
                        type="file"
                        onChange={(e) => setFile(e.target.files[0])}
                        className="text-sm"
                    />

                    <button
                        onClick={uploadFile}
                        className="bg-green-500 px-4 py-2 rounded hover:bg-green-600"
                    >
                        {loading ? "Uploading..." : "Upload"}
                    </button>
                </div>

                {file && (
                    <div className="mt-3 text-sm text-gray-300">
                        Selected: {file.name}
                    </div>
                )}
            </div>

            <div className="grid grid-cols-3 gap-6">
                {/* LEFT PANEL */}
                <div className="col-span-1 space-y-4">
                    <input
                        type="text"
                        placeholder="Search documents..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        className="w-full p-2 rounded bg-black border border-gray-700 text-sm"
                    />

                    {filteredDocs.map((doc, i) => (
                        <div
                            key={i}
                            onClick={() => loadInsights(doc.fileName)}
                            className="glass p-4 rounded cursor-pointer hover:scale-105 transition"
                        >
                            <p className="font-semibold truncate">
                                {doc.fileName}
                            </p>
                            <p className="text-xs text-green-400 mt-2">
                                Indexed ✅
                            </p>
                        </div>
                    ))}
                </div>

                {/* RIGHT PANEL */}
                <div className="col-span-2 space-y-6">

                    {/* 🔥 RISK SCORE */}
                    {selectedDoc && (
                        <div className="glass p-4 rounded">
                            <h2 className="font-semibold mb-2">
                                Risk Score
                            </h2>

                            <div className="text-4xl font-bold text-red-400">
                                {riskScore}%
                            </div>

                            <p className="text-sm text-gray-400">
                                Based on detected risk insights
                            </p>
                        </div>
                    )}

                    {/* 🔥 BAR CHART */}
                    <div className="glass p-4 rounded">
                        <h2 className="mb-4 font-semibold">
                            Insights Distribution
                        </h2>

                        {insights.length > 0 && (
                            <ResponsiveContainer width="100%" height={250}>
                                <BarChart data={chartData}>
                                    <CartesianGrid strokeDasharray="3 3" />
                                    <XAxis dataKey="name" />
                                    <YAxis />
                                    <Tooltip />
                                    <Bar dataKey="value" fill="#22c55e" />
                                </BarChart>
                            </ResponsiveContainer>
                        )}
                    </div>

                    {/* 🔥 TABS */}
                    <div className="flex gap-3">
                        {["All", "Risk", "Growth", "Opportunity"].map(tab => (
                            <button
                                key={tab}
                                onClick={() => setActiveTab(tab)}
                                className={`px-3 py-1 rounded text-sm ${activeTab === tab
                                        ? "bg-green-500"
                                        : "bg-gray-700"
                                    }`}
                            >
                                {tab}
                            </button>
                        ))}
                    </div>

                    {/* 🔥 INSIGHTS */}
                    <div className="glass p-4 rounded">
                        {insightLoading ? (
                            <p className="text-gray-400">
                                🔄 Analyzing document...
                            </p>
                        ) : filteredInsights.length === 0 ? (
                            <p className="text-gray-400">
                                No insights found
                            </p>
                        ) : (
                            <div className="space-y-3">
                                {filteredInsights.map((item, i) => (
                                    <div
                                        key={i}
                                        className="bg-white/5 p-4 rounded border border-white/10"
                                    >
                                        <h3 className="font-bold">
                                            {item.title}
                                        </h3>

                                        <p className="text-sm text-gray-300">
                                            {item.description}
                                        </p>

                                        <span
                                            className={`text-xs mt-2 inline-block px-2 py-1 rounded
                                            ${item.type === "Risk"
                                                    ? "bg-red-500/20 text-red-400"
                                                    : item.type === "Growth"
                                                        ? "bg-green-500/20 text-green-400"
                                                        : "bg-blue-500/20 text-blue-400"
                                                }`}
                                        >
                                            {item.type}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Documents;