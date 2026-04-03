import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../components/Sidebar';
import ChatBox from '../components/ChatBox';
import { useAuth } from '../contexts/AuthContext';
import { fetchMessages, fetchUsers } from '../api/chatApi';
import useSignalR from '../hooks/useSignalR';

export default function ChatPage() {
  const [users, setUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [messages, setMessages] = useState([]);
  const [onlineUsers, setOnlineUsers] = useState([]);
  const { user, token, logout } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const loadUsers = async () => {
      const data = await fetchUsers();
      setUsers(data);
    };

    loadUsers().catch(console.error);
  }, []);

  useEffect(() => {
    if (!selectedUser) return;
    fetchMessages(selectedUser.id)
      .then(setMessages)
      .catch(console.error);
  }, [selectedUser]);

  const onReceiveMessage = useCallback((message) => {
    if (!selectedUser) return;
    const involvesActiveChat =
      (message.senderId === selectedUser.id && message.receiverId === user.id) ||
      (message.senderId === user.id && message.receiverId === selectedUser.id);

    if (involvesActiveChat) {
      setMessages((prev) => [...prev, message]);
    }
  }, [selectedUser, user?.id]);

  const { sendMessage } = useSignalR(token, user?.id, onReceiveMessage, setOnlineUsers);

  const onSendMessage = async (content) => {
    if (!selectedUser) return;
    await sendMessage(selectedUser.id, content);
  };

  const onLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <main className="h-screen md:p-4">
      <div className="h-full mx-auto max-w-7xl rounded-none md:rounded-2xl overflow-hidden shadow-lg bg-white flex flex-col md:flex-row">
        <Sidebar
          users={users}
          selectedUser={selectedUser}
          onSelectUser={setSelectedUser}
          onlineUsers={onlineUsers}
          currentUser={user}
          onLogout={onLogout}
        />
        <ChatBox
          user={selectedUser}
          messages={messages}
          currentUserId={user?.id}
          onSendMessage={onSendMessage}
        />
      </div>
    </main>
  );
}
