export interface ITenantViewModel {
  id: string;
  code: string;
  name: string;
  logoUrl?: string;
  taxId?: string;
  companyName?: string;
  companyAddress?: string;
  createdAt?: Date;
}

export interface IUpdateTenantRequest {
  code: string;
  name: string;
  logoUrl?: string;
  taxId?: string;
  companyName?: string;
  companyAddress?: string;
}
