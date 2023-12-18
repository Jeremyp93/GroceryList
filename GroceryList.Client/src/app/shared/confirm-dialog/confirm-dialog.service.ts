import { Injectable, signal } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({providedIn: 'root'})
export class ConfirmDialogService {
    dialogQuestion = signal<string | null>(null);
    #answer: Subject<boolean> = new Subject<boolean>();
    answer$: Observable<boolean> = this.#answer.asObservable();

    setQuestion = (text: string) => {
        this.dialogQuestion.set(text);
    }

    clear = () => {
        this.dialogQuestion.set(null);
    }

    setAnswer = (answer: boolean) => {
        this.#answer.next(answer);
        this.clear();
    }
}