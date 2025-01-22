import { userManager } from '@/config';
import axios from 'axios';

const apiClient = axios.create({
    baseURL: `${import.meta.env.VITE_API_URL}/api/v1`,
    headers: {
        'Content-Type': 'application/json',
    },
});

// add a request interceptor to get token from auth store
apiClient.interceptors.request.use(
    async (config) => {
        const user = await userManager.getUser();

        if (user && user.access_token) {
            config.headers["Authorization"] = `Bearer ${user.access_token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

const refreshToken = async () => {
    try {
        const user = await userManager.signinSilent();
        return user?.access_token;
    } catch (e) {
        console.log("Error", e);
    }
};

// response parse
apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    async function (error) {
        const originalRequest = error.config;
        if (error.response.status === 403 && !originalRequest._retry) {
            originalRequest._retry = true;

            const newAccessToken = await refreshToken();

            const access_token = newAccessToken;

            apiClient.defaults.headers.common[
                "Authorization"
            ] = `Bearer ${access_token}`;
            return apiClient(originalRequest);
        }
        return Promise.reject(error);
    }
);

export default apiClient;