export const CONFIGURATION = {
  auth: {
    url: 'api/auth',
    token: {
      key: 'token',
      id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
      name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
      role: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    }
  },
  album : {
    url: 'api/albums',
    pageSize: 5
  },
  image : {
    url: 'api/images',
    pageSize: 5
  },
  roles: {
    admin: 'Admin',
    user: 'User'
  }
};