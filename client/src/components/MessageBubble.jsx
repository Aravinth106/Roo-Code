export default function MessageBubble({ message, own }) {
  return (
    <div className={`flex ${own ? 'justify-end' : 'justify-start'}`}>
      <div className={`max-w-[75%] px-4 py-2 rounded-2xl shadow-sm ${own ? 'bg-emerald-500 text-white rounded-br-sm' : 'bg-white text-slate-800 rounded-bl-sm'}`}>
        <p>{message.content}</p>
        <p className={`text-[10px] mt-1 ${own ? 'text-emerald-50' : 'text-slate-400'}`}>
          {new Date(message.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
        </p>
      </div>
    </div>
  );
}
