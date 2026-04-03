import { useState } from 'react';

export default function InputBox({ disabled, onSend }) {
  const [value, setValue] = useState('');

  const submit = (event) => {
    event.preventDefault();
    const trimmed = value.trim();
    if (!trimmed) return;
    onSend(trimmed);
    setValue('');
  };

  return (
    <form onSubmit={submit} className="p-4 border-t border-slate-200 bg-white flex gap-2">
      <input
        className="flex-1 rounded-full border border-slate-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-emerald-400"
        placeholder={disabled ? 'Select a user to start chatting' : 'Type a message...'}
        disabled={disabled}
        value={value}
        onChange={(event) => setValue(event.target.value)}
      />
      <button
        type="submit"
        disabled={disabled}
        className="rounded-full bg-emerald-500 text-white px-4 py-2 disabled:bg-slate-300"
      >
        Send
      </button>
    </form>
  );
}
