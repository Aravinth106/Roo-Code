export default function Sidebar({ users, selectedUser, onSelectUser, onlineUsers, currentUser, onLogout }) {
  return (
    <aside className="w-full md:w-80 border-r border-slate-200 bg-white flex flex-col">
      <div className="p-4 border-b border-slate-200 flex items-center justify-between">
        <div>
          <p className="text-sm text-slate-500">Signed in as</p>
          <p className="font-semibold text-slate-800">{currentUser?.name}</p>
        </div>
        <button type="button" className="text-sm text-red-500" onClick={onLogout}>Logout</button>
      </div>
      <div className="overflow-y-auto">
        {users.map((user) => {
          const isSelected = selectedUser?.id === user.id;
          const online = onlineUsers.includes(user.id);
          return (
            <button
              key={user.id}
              className={`w-full px-4 py-3 text-left border-b border-slate-100 flex items-center justify-between ${isSelected ? 'bg-emerald-50' : 'hover:bg-slate-50'}`}
              onClick={() => onSelectUser(user)}
              type="button"
            >
              <span className="font-medium text-slate-700">{user.name}</span>
              <span className={`text-xs px-2 py-1 rounded-full ${online ? 'bg-emerald-100 text-emerald-700' : 'bg-slate-100 text-slate-500'}`}>
                {online ? 'Online' : 'Offline'}
              </span>
            </button>
          );
        })}
      </div>
    </aside>
  );
}
