import { useEffect, useState } from "react";
import API from "../api/axios";

function Insights() {
    const [documents, setDocuments] = useState([]);
    const [selectedDoc, setSelectedDoc] = useState(null);
    const [insights, setInsights] = useState([]);

    useEffect(() => {
        fetchDocs();
    }, []);

    const fetchDocs = async () => {
        try {
            const res = await API.get("/Document/my-documents");
            setDocuments(res.data);
        } catch (err) {
            console.error(err);
        }
    };

    const loadInsights = async (fileName) => {
        try {
            const res = await API.get(
                `/Insights?query=${fileName} financial risks growth opportunities`
            );
            setInsights(res.data);
            setSelectedDoc(fileName);
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <div>
            <h1 className="text-2xl font-bold mb-6">Insights</h1>

            <div className="grid grid-cols-3 gap-6">
                {/* DOCUMENTS */}
                <div className="glass p-4 rounded">
                    <h2 className="mb-4 font-semibold">Documents</h2>

                    {documents.map((doc, i) => (
                        <div
                            key={i}
                            onClick={() => loadInsights(doc.fileName)}
                            className="cursor-pointer p-2 rounded hover:bg-white/10"
                        >
                            {doc.fileName}
                        </div>
                    ))}
                </div>

                {/* INSIGHTS */}
                <div className="col-span-2 glass p-4 rounded">
                    <h2 className="mb-4 font-semibold">
                        {selectedDoc || "Select a document"}
                    </h2>

                    {insights.length === 0 ? (
                        <p className="text-gray-400">
                            No insights loaded yet...
                        </p>
                    ) : (
                        <div className="space-y-3">
                            {insights.map((item, i) => (
                                <div
                                    key={i}
                                    className="bg-white/5 p-3 rounded"
                                >
                                    <h3 className="font-bold">
                                        {item.title}
                                    </h3>
                                    <p className="text-sm">
                                        {item.description}
                                    </p>

                                    <span className="text-xs px-2 py-1 bg-gray-700 rounded">
                                        {item.type}
                                    </span>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

export default Insights;