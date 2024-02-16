import { Injectable } from '@angular/core';
import { environment } from '@app-env/environment';
import { HttpParams } from '@angular/common/http';

@Injectable()
export class BaseService {
  protected host = environment.host;
  protected endpoint!: string;

  constructor() {}

  protected setEndpoint(controller: string) {
    this.endpoint = `${this.host}/api/v1/${controller}`;
  }

  protected getApiUrl(action?: string) {
    return action ? `${this.endpoint}/${action}` : this.endpoint;
  }

  protected createParams(params?: { [key: string]: any }) {
    if (!params) return;
    Object.keys(params).forEach((key) => {
      if (params[key] === undefined || params[key] === null) delete params[key];
    });
    return Object.entries(params).reduce(
      (params, [key, value]) => params.set(key, value),
      new HttpParams(),
    );
  }
}
