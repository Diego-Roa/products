import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { switchMap, filter, take } from 'rxjs/operators';

export const tokenInterceptor: HttpInterceptorFn = (request, next) => {
  const whitelist: string[] = ['/login'];
  const auth = inject(AuthService);

  const isWhitelisted = whitelist.some(path => request.url.includes(path));
  if (isWhitelisted) {
    return next(request);
  }

  return auth.getToken().pipe(
    filter(token => token !== null),
    take(1),
    switchMap(token => {
      const authRequest = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
      });
      return next(authRequest);
    })
  );
};
