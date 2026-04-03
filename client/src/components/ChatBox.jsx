import { useEffect, useRef } from 'react';
import MessageBubble from './MessageBubble';
import InputBox from './InputBox';

export default function ChatBox({ user, messages, currentUserId, onSendMessage }) {
  const endRef = useRef(null);

  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages, user?.id]);

  return (
    <section className="flex-1 flex flex-col bg-slate-100">
      <header className="px-6 py-4 border-b border-slate-200 bg-white">
        <h2 className="font-semibold text-slate-800">{user ? user.name : 'Select a conversation'}</h2>
      </header>

      <div className="flex-1 overflow-y-auto p-4 space-y-3">
        {messages.map((message) => (
          <MessageBubble key={message.id || `${message.senderId}-${message.timestamp}`} message={message} own={message.senderId === currentUserId} />
        ))}
        <div ref={endRef} />
      </div>

      <InputBox disabled={!user} onSend={onSendMessage} />
    </section>
  );
}
