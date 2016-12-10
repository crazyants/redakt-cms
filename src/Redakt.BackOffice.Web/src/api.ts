import {autoinject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import {Router} from 'aurelia-router';
//import Cookies = require('cookies-js');
import {User} from './models/user';

@autoinject()
export class Api {
    constructor(private http: HttpClient, private router: Router, private user: User) {
        http.configure(config => { config
            .withBaseUrl(this.apiBase)
            .withDefaults({
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json'
                }
            });            
        });
    }

	private apiClientId = '526ce7f8cc6f100c78c15a7f';
    private apiBase = 'http://localhost:59522/redakt/api/';
    private tokenEndpoint = 'https://localhost:44349/connect/token';
    //private apiBase = 'http://loggitapi.azurewebsites.net';
    private tokenCookieKey = 'REDAKT.TOKEN';
    private accessToken: string = null;

    public isAuthenticated(): boolean {
        if (!this.accessToken) {
            var token = 'dummy';
            //var token = Cookies.get(this.tokenCookieKey);
            if (token) {
                this.accessToken = token;
                this.configureHttp(token);
            }
        }

        return !!this.accessToken;
    }

	public authenticate(username: string, password: string, rememberMe = false) {
	    return this.getToken(username, password)
	        .then(token => {
                this.accessToken = token;
	            this.configureHttp(token);
                //Cookies.set(this.tokenCookieKey, token, rememberMe ? { expires: 43200 } : null);
            })
            .catch(e => alert('Invalid login.'));
	}

	public googleAuthenticate() {
        this.router.navigate(this.apiBase + '/oauth/external?provider=Google');
	    //window.location.href = this.apiBase + '/users/auth/external?provider=Google';
	}

    private getToken(username: string, password: string): Promise<string> {
        return this.http.fetch(this.tokenEndpoint, {
            method: 'post',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: 'grant_type=password&client_id=' + this.apiClientId + '&client_secret=32432&username=' + username + '&password=' + password
        }).then(r => {
            return r.json();
        }).then(json => {
            return (<any>json).access_token;
        });
    }

    private configureHttp(accessToken: string) {
        this.http.configure(config => { config
            .withBaseUrl(this.apiBase)
            .withDefaults({
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + accessToken
                }
            });            
        });
    }

    //private loadUser() {
    //    return this.get('/users/me')
    //        .then((u: IHttpResponseMessage<IUserDto>) => {
    //            this.user.employeeId = u.content.organisations[0].employeeId;
    //            return u.content;
    //        });
    //}

    //private tokenFromRedirect(): string {
    //    if (!this.$location.hash()) return null;
    //    var token = null;
    //    var expiresIn = null;
    //    $.each(document.location.hash.substr(1).split('&'), function (index, value) {
    //        var pair = value.split('=');
    //        if (pair[0] == 'access_token') {
    //            token = pair[1];
    //        }
    //        else if (pair[0] == 'expires_in') {
    //            expiresIn = pair[1];
    //        }
    //    });

    //    if (!!token) {
    //        this.$cookies[this.tokenCookieKey] = token;
    //        this.$location.url('/');
    //    }
    //    return token;
    //}

    //private authenticationHeader(): string {
    //    if (!this.accessToken) {
    //        this.accessToken = this.tokenFromRedirect();
    //        if (!this.accessToken) this.accessToken = this.$cookies[this.tokenCookieKey];
    //        if (!this.accessToken) return null;
    //    }
    //    return 'Bearer ' + this.accessToken;
    //}

    private redirectToLogin(): void {
        this.router.navigate('signin', false);
    }

    private notAuthorized(): void {
        this.redirectToLogin();
    }

    //public isAuthenticated(): boolean {
    //    return this.authenticationHeader() > '';
    //}

   // public call(method, url, data = null): Promise<any> {
   //     var deferred = this.$q.defer();

   //     this.http({
   //         method: method,
   //         url: this.apiBase + url,
   //         data: data
   //     }).then(response => {
   //         if (response) deferred.resolve(response.data);
   //         else deferred.reject('server error');
   //     },(error) => {
			//	deferred.reject(error.statusText);
			//});

   //     return deferred.promise;
   // }

    public get(url: string): Promise<any> {
		return this.http.fetch(url)
            .then(r => r.json())
            .catch(e => alert('Error.'));
    }

    public post(url: string, data): Promise<any> {
        return this.http.fetch(url, {
            method: 'post',
            body: json(data)
        }).then(r => {
            return r.json();
        }).catch(e => {
            alert('Error');
        });
    }

    public put(url: string, data): Promise<any> {
        return this.http.fetch(url, {
            method: 'put',
            body: json(data)
        }).then(r => {
            return;// r.json();
        }).catch(e => {
            alert('Error');
        });
    }

    public delete(url: string): Promise<any> {
        return this.http.fetch(url, {
            method: 'delete'
        }).then(r => {
            return r.json();
        }).catch(e => {
            alert('Error');
        });
    }
}
