import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';

@inject(Router)
export class Dashboard {
    constructor(private router: Router) {
		//debugger;
    }

    public title = 'dsajkodjhsal';

    public activate(params, config, instruction) {
    }
}