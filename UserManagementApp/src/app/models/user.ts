export interface User {
  id: string;
  firstName: string;
  lastName: string;
  contact?: string;
  country?: string;
  email: string;
  consentGiven: boolean;
  consentDate?: Date;
}
