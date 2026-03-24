import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import API from "../api/axios";

function InsightDetail() {
    const { fileName } = useParams();
    const [insights, setInsights] = useState([]);

    useEffect(() => {
        fetchInsights();
    }, []);

    const fetchInsights = async () => {
        try {
            const res = await API.get(
                "/Insights?query=financial performance risks growth opportunities insights"
            );

            // 🔥 TEMP: show all insights (backend doesn't filter yet)
            setInsights(res.data);
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <Layout>
            <h1 className="text-3xl font-bold mb-6">
                📄 {decodeURIComponent(fileName)}
            </h1>

            <div className="space-y-4">
                {insights.map((item, i) => (
                    <div
                        key={i}
                        className="glass p-5 rounded border border-white/10"
                    >
                        <h3 className="font-bold">{item.title}</h3>
                        <p className="text-sm">{item.description}</p>

                        <span
                            className={`mt-2 inline-block px-3 py-1 rounded text-xs ${item.type === "Risk"
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
        </Layout>
    );
}

export default InsightDetail;