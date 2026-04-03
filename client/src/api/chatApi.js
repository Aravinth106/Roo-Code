import api from './http';

export const fetchUsers = async () => {
  const { data } = await api.get('/api/users');
  return data;
};

export const fetchMessages = async (userId) => {
  const { data } = await api.get(`/api/messages/${userId}`);
  return data;
};
