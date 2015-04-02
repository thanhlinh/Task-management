:host {
      display: inline-block;
      position: relative;
      width: 400px;
    }

    .mirror-text {
      visibility: hidden;
    }

    ::content textarea {
      padding: 0;
      margin: 0;
      border: none;
      outline: none;
      resize: none;
      /* see comments in template */
      width: 100%;
      height: 100%;
    }

    ::content textarea:invalid {
      box-shadow: none;
    }