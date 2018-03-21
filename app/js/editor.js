// Save Monaco's amd require and restore Node's require
var amdRequire = global.require;
global.require = nodeRequire;

// require node modules before loader.js comes in
var path = require('path');
function uriFromPath(_path) {
  var pathName = path.resolve(_path).replace(/\\/g, '/');
  if (pathName.length > 0 && pathName.charAt(0) !== '/') {
    pathName = '/' + pathName;
  }
  return encodeURI('file://' + pathName);
}
amdRequire.config({
  baseUrl: uriFromPath(path.join(__dirname, '../node_modules/monaco-editor/min'))
});
// workaround monaco-css not understanding the environment
self.module = undefined;
// workaround monaco-typescript not understanding the environment
self.process.browser = true;
amdRequire(['vs/editor/editor.main'], function () {

  monaco.languages.register({
    id: 'arm'
  });
  monaco.languages.setMonarchTokensProvider('arm', {
    // Set defaultToken to invalid to see what you do not tokenize yet
    defaultToken: 'invalid',

    ignoreCase: true,

    brackets: [
      ['{', '}', 'delimiter.curly'],
      ['[', ']', 'delimiter.square'],
      ['(', ')', 'delimiter.parenthesis']
    ],

    operators: [
      '=', '>', '!'
    ],

    keywords: [
      'MOV', 'ADD', 'ADC', 'MVN', 'ORR', 'AND', 'EOR', 'BIC', 'SUB', 'SBC',
      'RSB', 'RSC', 'CMP', 'CMN', 'TST', 'TEQ', 'LSL', 'LSR', 'ASR', 'LDR',
      'STR', 'ADR', 'LDM', 'STM', 'MVNS', 'MOVS', 'MOV', 'SUBS', 'BPL', 'MVNS',
      'ADD', 'SUBS', 'CMP', 'BNE', 'MOVS', 'ASR', 'CMPCS', 'RRX', 'ASR', 'ROR',
      'ADCS', 'BEQ', 'B'
    ],

    // we include these common regular expressions
    symbols: /[=><!~?:&|+\-*\/\^%]+/,

    // C# style strings
    escapes: /\\(?:[abfnrtv\\"']|x[0-9A-Fa-f]{1,4}|u[0-9A-Fa-f]{4}|U[0-9A-Fa-f]{8})/,

    // The main tokenizer for our languages
    tokenizer: {
      root: [
        // identifiers and keywords
        [/[a-z_$][\w$]*/, {
          cases: {
            '@keywords': 'keyword',
            '@default': 'identifier'
          }
        }],

        // whitespace
        { include: '@whitespace' },

        // delimiters and operators
        [/[{}()\[\]]/, '@brackets'],
        [/[<>](?!@symbols)/, '@brackets'],
        [/@symbols/, {
          cases: {
            '@operators': 'operator',
            '@default': ''
          }
        }],

        // @ annotations.
        // As an example, we emit a debugging log message on these tokens.
        // Note: message are supressed during the first load -- change some lines to see them.
        [/@\s*[a-zA-Z_\$][\w\$]*/, { token: 'annotation', log: 'annotation token: $0' }],

        // numbers
        [/\d*\.\d+([eE][\-+]?\d+)?/, 'number.float'],
        [/0[xX][0-9a-fA-F]+/, 'number.hex'],
        [/0[b][0-1]+/, 'number.bin'],
        [/\d+/, 'number'],

        // // delimiter: after number because of .\d floats
        [/[,.]/, 'delimiter'],
        [/\;+.*/, 'comment', '@push'],

        // strings
        [/"([^"\\]|\\.)*$/, 'string.invalid'],  // non-teminated string
        [/"/, { token: 'string.quote', bracket: '@open', next: '@string' }],

        // characters
        [/'[^\\']'/, 'string'],
        [/(')(@escapes)(')/, ['string', 'string.escape', 'string']],
        [/'/, 'string.invalid'],

      ],


      string: [
        [/[^\\"]+/, 'string'],
        [/@escapes/, 'string.escape'],
        [/\\./, 'string.escape.invalid'],
        [/"/, { token: 'string.quote', bracket: '@close', next: '@pop' }]
      ],

      whitespace: [
        [/[ \t\r\n]+/, 'white'],
        //        [/\/\*/, 'comment', '@comment'],
        //        [/\/\/.*$/, 'comment'],
      ],
    }
  });

  monaco.editor.defineTheme('customVisualTheme', {
    base: 'vs-dark', // can also be vs-dark or hc-black
    inherit: true, // can also be false to completely replace the builtin rules
    rules: [
      { token: 'comment', foreground: '8c8c8c', fontStyle: 'italic' },
    ]
  });

  window.code = monaco.editor.create(document.getElementById('editor'), {
    value: [
      'ADD R0, R0, #1'
    ].join('\n'),
    language: 'arm',
    theme: 'customVisualTheme',
    renderWhitespace: 'all',
    roundedSelection: false,
    scrollBeyondLastLine: false
  });

  window.setError = function (err, line){
    var startIndex = 0;
    var errorMarker =
      [{
        severity: monaco.Severity.Error,
        startLineNumber: line,
        startColumn: 1,
        endLineNumber: line,
        endColumn: 1000,
        message: err
      }];
    
    var model = monaco.editor.getModels()[0];
    
    monaco.editor.setModelMarkers(model, "arm", errorMarker);
  };

  window.clearError = function(holder){
    var errorMarker = [{}];
    console.log("clear markers");
    var model = monaco.editor.getModels()[0];
    
    monaco.editor.setModelMarkers(model, "arm", errorMarker);
  };

});
