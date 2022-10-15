import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'ProcessManagement',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44378/',
    redirectUri: baseUrl,
    clientId: 'ProcessManagement_App',
    responseType: 'code',
    scope: 'offline_access ProcessManagement',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44378',
      rootNamespace: 'EasyAbp.ProcessManagement',
    },
    ProcessManagement: {
      url: 'https://localhost:44386',
      rootNamespace: 'EasyAbp.ProcessManagement',
    },
  },
} as Environment;
