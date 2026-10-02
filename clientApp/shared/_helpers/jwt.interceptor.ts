import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpXsrfTokenExtractor} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserDataService} from '../../_services/user-data.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private xsrfTokenExtractor: HttpXsrfTokenExtractor,private  userDataService:UserDataService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
     const xsrfToken = this.xsrfTokenExtractor.getToken();
    // add authorization header with jwt token if available
    const currentUserToken = this.userDataService.getCurrentUser(false);
    if (currentUserToken) {
      try {
        const currentUser = JSON.parse(currentUserToken);
        if (currentUser && currentUser.token) {
          request = request.clone({
            setHeaders: {Authorization: `Bearer ${currentUser.token}`}
          });
        }
      } catch (e) {
        console.log('Invalid User Token');
      }
    }

    if (['POST'].includes(request.method) && xsrfToken) {
      request = request.clone({
        withCredentials: true,
        headers: request.headers.set('XSRF-TOKEN', xsrfToken)
      });
    }

    // if (request.method === 'POST') {
    //     request = request.clone({
    //         withCredentials: true,
    //         headers: request.headers.set('X-XSRF-TOKEN', xsrfToken)
    //     });
    // }
    return next.handle(request);
  }
}
