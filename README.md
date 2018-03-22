# ARM Emulator GUI

> Front end code for ARM emulator with backend written in F# at https://github.com/XavKearney/fsharp-arm-emulator

## Introduction
This project is based on a starter template from https://github.com/filangel/arm-monaco.
The target language is `F#`, which is transpiled to `Javascript` (`js`) thanks to [Fable](https://fable.io).
[Github Electron](https://electronjs.org/) is then used to convert the developed web-app to a cross-platform native application,
providing access to platform-level commands (i.e. file-system, path, multiple processes), which are unavailable to
(vanilla) browser web-apps.
[Webpack](https://webpack.js.org/) is the module bundler, responsible for the transpilation and automated building process.
Finally, [Monaco Editor](https://microsoft.github.io/monaco-editor/) is used for as a self-contained javascript component that implements an editor window which your `F#` code can interact with.

## Features

This GUI seeks to emulate the visUAL implementation at https://salmanarif.bitbucket.io/visual/ as closely as possbile while making improvements and changes.  To be able to make a clear comparison between this and the existing implementation, the front end has been styled fairly similarly.  The key difference is making flags and memory easily accessible in the right panel with the registers.  We believed that flag and memory access was clunky in visUAL, hence the change.  

Below are the list of features (not) implemented in this GUI.

| Feature | Implemented | Different to visUAL |
| :---:|:---:|:---:|
| New file | ✔️ |
| Open file | ✔️ |
| Save file | ✔️ |
| Copy registers (by clicking on the register value) | ✔️ | ✔️ |
| Randomise registers | ✔️ | ✔️ |
| Change formatting of individual registers | ✔️ |
| Change formatting of all registers at once | ✔️ | ✔️
| Labels | ✔️ |
| Memory | ✔️ |
| Run code | ✔️ |
| Reset registers and flags | ✔️ |
| Reset and run | ✔️ | ✔️ |
| Infinite branch detection | ✔️ |
| Breakpoints | ✖️ |
| Step forwards/backwards | ✖️ |
| Inline errors | ✔️ | 
| Set flags by clicking | ✔️ | ✔️
| Clock cycles | ✖️ |
| Change font size | ✔️ |
| Persistent registers and flags | ✔️ | ✔️
| Execution/Error status | ✔️ | 


## Tests

No tests have been written for this front end code.  The backend is extensively tested though at https://github.com/XavKearney/fsharp-arm-emulator.  So you can be sure that the emulator works almost perfectly, however there may still be some front end bugs/glitches.


## Issues

During our use of the GUI doing manual testing with random code snippets and cases, we discovered an issue with Fable in regards to the stack.  In the backend emulator code (specifically TopLevel.fs) we detect a possible infinite loop (due to branching for example) when the code is looped 100,000 times.  However, when compiling this code to JavaScript using Fable, an infinite loop throws an error in the console.  Upon investigation we discovered that this error is due to the fact that Fable compiles tail recursive functions to deep stack calls.  As a result the electron app runs out of stack space after about 500 branches.  This is a limiting factor since large programs will branch more than 500 times.  

Unfortunately there is no easy fix for this compilation error since it is caused by the way Fable functions.  The only fix we could think of was to convert the tail recursive function in TopLevel to a set of FOR loops since Fable will be able to compile this properly.  However this goes against the functional nature of F# and would make the code less readable and more messy than before.  Overall, this issue is a limiting factor when considering this prject as a fully fledged replacement for visUAL.


## Dependencies

Before proceeding any further, make sure these packages are installed/setup to your machine:

1. [`dotnet core`](https://www.microsoft.com/net/learn/get-started/)

2. [`node.js`](https://nodejs.org/en/download/): `Javascript` runtime engine

3. [`yarn`](https://yarnpkg.com/en/docs/install): `node.js` package manager

4. (**non-Windows only**) [`mono`](http://www.mono-project.com/docs/getting-started/install/)

5. [`fsharp`](http://fsharp.org/use/)

## Project Structure

### `package.json`

Electron bundles [Chromium](https://www.chromium.org/) (View) and [node.js](https://nodejs.org/en/) (Engine),
therefore as every `node.js` project, the `package.json` file specifies the module dependencies.
Additionally, the section `"scripts"`:

```json
{
    ...
    "scripts": {
        "start": "cd src/Main && dotnet fable webpack --port free -- -w --config webpack.config.js",
        "build": "cd src/Main && dotnet fable webpack --port free -- -p --config webpack.config.js",
        "launch": "electron ."
    },
    ...
}
```

is defining the in-project shortcut commands, therefore when we use `yarn run <stript_key>` is equivalent
to calling `<script_value>`. For example, in the root of the project, running in the terminal
`yarn run launch` is equivalent to running `electron .`.

### `webpack.config.js`

`Webpack` configuration file, called when `yarn run start` is executed, firing a process that watches changes
to `src` folder files and transpiled the `F#` project to `js` on save.
For example, the `main.js` file is generated by:

```js
var mainConfig = Object.assign({
  target: "electron-main",
  entry: resolve("src/Main/Main.fsproj"),
  output: {
    path: resolve("."),
    filename: "main.js"
  }
}, basicConfig);
```

that selects the `F#` project at `src/Main/Main.fsproj`, transpiles it using `Fable` and saves the
generated `js` file at `main.js`.
The `rendered.js` file is generated similarly, using this configuration:

```js
var rendererConfig = Object.assign({
  target: "electron-renderer",
  devtool: "source-map",
  entry: resolve("src/Renderer/Renderer.fsproj"),
  output: {
    path: resolve("app/js"),
    filename: "renderer.js"
  }
}, basicConfig);
```

### `src` folder

#### `src/Emulator`

The emulator source `F#` code. This is used as submodule (from https://github.com/XavKearney/fsharp-arm-emulator) 

### `app` folder

The web-app, view, project files.

#### `app/index.html`

The markup code for the view.
`src/Renderer/Ref.fs` module accesses the elements defined in this DOM tree.

#### `app/css`

`CSS` code to prettify the `index.html` elements.

#### `app/js`

The `js` scripts loaded by the `index.html`, **after** the DOM elements (statically defined) are rendered.

##### `app/js/editor.js`

`Monaco Editor` setup script.

## Getting Started

1. Fetch `npm` packages by executing `yarn install`

2. Run `setup.bat` (on Windows). This downloads and updates the submodule, and installs their packages individually (necessary because of the submodule structure), then restores the global packages.

3. Compile `fsharp` code to `javascript` using `webpack` by executing `yarn run start`

4. Open `electron` application at a new terminal tab by running `yarn run launch`