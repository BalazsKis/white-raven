import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatSnackBar } from '@angular/material';

import { ShareService } from '../../services/share.service';
import { Share } from '../../models/share';
import { ConfirmDeleteComponent } from '../confirm-delete/confirm-delete.component';

@Component({
  selector: 'wr-view-share',
  templateUrl: './view-share.component.html',
  styleUrls: ['./view-share.component.scss']
})
export class ViewShareComponent implements OnInit {

  noteId: string;
  canRemoveShare: boolean;

  isShareListLoading = true;

  shareList: Share[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private shareService: ShareService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar) {
    this.noteId = data.noteId;
    this.canRemoveShare = data.canRemoveShare;
  }

  private loadShares(): void {
    this.isShareListLoading = true;

    this.shareService.getSharesForNote(this.noteId)
      .subscribe(shares => {
        this.shareList = shares.sort((a, b) => b.contributionType - a.contributionType);
        this.isShareListLoading = false;
      });
  }

  removeShare(share: Share): void {
    const dialogRef = this.dialog.open(ConfirmDeleteComponent,
      {
        data: {
          objectName: 'user',
          deleteOperation: () => this.shareService.deleteShare(this.noteId, share.user, share.contributionType)
        }
      });

    dialogRef.afterClosed().subscribe(isDeleted => {
      if (isDeleted) {
        this.loadShares();
        this.snackBar.open('User removed', null, { duration: 1500 });
      }
    });
  }

  ngOnInit() {
    this.loadShares();
  }

}
