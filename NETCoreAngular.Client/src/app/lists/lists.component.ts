import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit
{
    memebrs: Member[] | undefined;
    predicate = 'liked';
    pageNumber = 1;
    pageSize = 3;
    pagination: Pagination | undefined;

    constructor(private memberService: MembersService) { }

    ngOnInit(): void
    {
        this.loadLikes();
    }

    loadLikes()
    {
        this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
            next: response =>
            {
                this.memebrs = response.result;
                this.pagination = response.pagination;
            }
        })
    }

    pageChanged(event: any)
    {

        if (this.pageNumber !== event.page)
        {
            this.pageNumber = event.page;
            this.loadLikes();
        }
    }
}
