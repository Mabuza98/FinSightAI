import { useEffect, useState } from "react";
import API from "../api/axios";

function Chat() {
    const [question, setQuestion] = useState("");
    const [messages, setMessages] = useState([]);
    const [documents, setDocuments] = useState([]);
    const [selectedDoc, setSelectedDoc] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchDocs = async () => {
            try {
                const res = await API.get("/Document/my-documents");
                setDocuments(res.data);
            } catch (err) {
                console.error(err);
            }
        };
        fetchDocs();
    }, []);

    const askAI = async () => {
        if (!question || !selectedDoc) {
            alert("Select document + enter question");
            return;
        }

        const userMsg = { role: "user", text: question };
        setMessages((prev) => [...prev, userMsg]);
        setLoading(true);

        try {
            const res = await API.post("/Ai/query", {
                question,
                fileName: selectedDoc,
            });

            const aiMsg = {
                role: "ai",
                text: res.data.answer,
            };

            setMessages((prev) => [...prev, aiMsg]);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
            setQuestion("");
        }
    };

    return (
        <div>
            <h1 className="text-2xl font-bold mb-4">AI Document Chat</h1>

            <select
                className="bg-gray-600 p-2 rounded mb-4"
                value={selectedDoc}
                onChange={(e) => setSelectedDoc(e.target.value)}
            >
                <option value="">Select Document</option>
                {documents.map((doc, i) => (
                    <option key={i} value={doc.fileName}>
                        {doc.fileName}
                    </option>
                ))}
            </select>

            <div className="glass p-4 rounded h-96 overflow-y-auto mb-4">
                {messages.map((msg, i) => (
                    <div key={i} className="mb-3">
                        <strong>
                            {msg.role === "user" ? "You" : "AI"}:
                        </strong>
                        <p>{msg.text}</p>
                    </div>
                ))}

                {loading && <p>🧠 Thinking...</p>}
            </div>

            <div className="flex gap-2">
                <input
                    className="bg-gray-600 flex-1 p-2 rounded"
                    value={question}
                    onChange={(e) => setQuestion(e.target.value)}
                    placeholder="Ask something..."
                />
                <button
                    onClick={askAI}
                    className="bg-green-500 px-4 rounded"
                >
                    Ask
                </button>
            </div>
        </div>
    );
}

export default Chat;