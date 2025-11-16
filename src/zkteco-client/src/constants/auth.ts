// ==========================================
// src/constants/auth.ts
// ==========================================

export const ACCESSTOKEN_KEY = 'access_token';
export const REFRESHTOKEN_KEY = 'refresh_token';

export const JWT_CLAIMS = {
    ROLE: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    NAME_IDENTIFIER: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
    NAME: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
    EMAIL: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
}