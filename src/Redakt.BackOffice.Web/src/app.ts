import {Aurelia, inject} from 'aurelia-framework';
import {Router, RouterConfiguration, NavigationInstruction, Redirect} from 'aurelia-router';
import {User} from './models/user';

@inject(User)
export class App {
    private router: Router;

    constructor(private user: User) {
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Redakt CMS';
        //config.addPipelineStep('authorize', AuthorizeStep);
        config.map([
            { name: 'dashboard', route: ['', 'dashboard'], moduleId: './views/dashboard', nav: true, title: 'Dashboard', icon: 'dashboard' },
            { name: 'pages', route: 'pages', moduleId: './views/pages', nav: true, title: 'Content', icon: 'content-copy' },
            { name: 'page', route: 'pages/:id', moduleId: './views/pages', nav: false, title: 'Content', icon: 'content-copy' },
            { name: 'resources', route: 'resources', moduleId: './views/dashboard', nav: true, title: 'Resources', icon: 'attachment' },
            { name: 'pagetypes', route: 'pagetypes', moduleId: './views/pagetypes', nav: true, title: 'Page Types', group: 'configuration' },
            { name: 'pagetype', route: 'pagetypes/:id', moduleId: './views/pagetypes', nav: false, title: 'Page Types', group: 'configuration' }
        ]);

        this.router = router;
    }

    public activate(params, config, instruction) {
    }
}

//@inject(Api, User)
//class AuthorizeStep {
//    constructor(private api: Api, private user: User) {
//    }

//    public run(navigationInstruction: NavigationInstruction, next) {
//        if (navigationInstruction.getAllInstructions().some(i => !(<any>i.config).isAnonymous)) {
//            if (!this.api.isAuthenticated()) {
//                return next.cancel(new Redirect('signin', null));
//            } else if (!this.user.isLoaded) {
//                return this.loadUser().then(() => {
//                    return next();
//                });
//            }
//        }

//        return next();
//    }

//    private loadUser() {
//        return this.api.get('ds')
//            .then(u => {
//                this.user.personId = u.personId;
//                this.user.organisationId = u.organisationId;
//                this.user.isLoaded = true;
//                this.user.username = u.username;
//                this.user.firstName = u.firstName;
//                return u;
//            });
//    }
//}
