"use client";
import { useState } from "react";

export default function LearnlyAssistant() {
  const [response, setResponse] = useState("");
  const [loading, setLoading] = useState(false);

  async function handlePrompt() {
    if (!("ai" in self) || !("prompt" in self.ai)) {
      alert("Chrome Prompt API not supported in this browser!");
      return;
    }

    setLoading(true);
    try {
      const session = await self.ai.prompt.create();
      const result = await session.prompt("Explain photosynthesis in simple terms.");
      setResponse(result);
    } catch (err) {
      console.error(err);
      alert("Failed to run AI prompt.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="p-6 max-w-lg mx-auto">
      <h1 className="text-xl font-semibold mb-4">Learnly Assistant</h1>
      <button 
        onClick={handlePrompt}
        className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        disabled={loading}
      >
        {loading ? "Thinking..." : "Ask Learnly"}
      </button>

      {response && (
        <div className="mt-4 p-4 bg-gray-100 rounded">
          <strong>AI Response:</strong>
          <p>{response}</p>
        </div>
      )}
    </div>
  );
}
