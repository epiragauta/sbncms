/// <reference types="Cypress" />
import { ScriptBuilder } from "sbn-cypress-testhelpers";

context('Scripts', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  function navigateToSettings() {
    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");
  }

  it('Create new JavaScript file', () => {
    const name = "TestScript";
    const fileName = name + ".js";

    cy.sbnEnsureScriptNameNotExists(fileName);

    navigateToSettings()

    cy.sbnTreeItem("settings", ["Scripts"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();
    cy.get('.menu-label').first().click(); // TODO: Fucked we cant use something like cy.sbnContextMenuAction("action-mediaType").click();

    //Type name
    cy.sbnEditorHeaderName(name);

    //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');
    cy.sbnScriptExists(fileName).should('be.true');


    //Clean up
    cy.sbnEnsureScriptNameNotExists(fileName);
  });

  it('Delete a JavaScript file', () => {
    const name = "TestDeleteScriptFile";
    const fileName = name + ".js";

    cy.sbnEnsureScriptNameNotExists(fileName);

    const script = new ScriptBuilder()
      .withName(name)
      .withContent('alert("this is content");')
      .build();

    cy.saveScript(script);

    navigateToSettings()

    cy.sbnTreeItem("settings", ["Scripts", fileName]).rightclick();
    cy.sbnContextMenuAction("action-delete").click();
    cy.sbnButtonByLabelKey("general_ok").click();

    cy.contains(fileName).should('not.exist');
    cy.sbnScriptExists(name).should('be.false');

    cy.sbnEnsureScriptNameNotExists(fileName);
  });

  it('Update JavaScript file', () => {
    const name = "TestEditJavaScriptFile";
    const nameEdit = "Edited";
    let fileName = name + ".js";

    const originalContent = 'console.log("A script);\n';
    const edit = 'alert("content");';
    const expected = originalContent + edit;

    cy.sbnEnsureScriptNameNotExists(fileName);

    const script = new ScriptBuilder()
      .withName(name)
      .withContent(originalContent)
      .build();
    cy.saveScript(script);

    navigateToSettings();
    cy.sbnTreeItem("settings", ["Scripts", fileName]).click();

    cy.get('.ace_text-input').type(edit, { force: true });

    // Since scripts has no alias it should be safe to not use sbnEditorHeaderName
    // sbnEditorHeaderName does not like {backspace}
    cy.get('#headerName').type("{backspace}{backspace}{backspace}" + nameEdit).should('have.value', name+nameEdit);
    fileName = name + nameEdit + ".js";
    cy.get('.btn-success').click();

    cy.sbnSuccessNotification().should('be.visible');
    cy.sbnVerifyScriptContent(fileName, expected).should('be.true');

    cy.sbnEnsureScriptNameNotExists(fileName);
  });

  it('Can Delete folder', () => {
    const folderName = "TestFolder";

    // The way scripts and folders are fetched and deleted are identical
    cy.sbnEnsureScriptNameNotExists(folderName);
    cy.saveFolder('scripts', folderName);

    navigateToSettings()

    cy.sbnTreeItem("settings", ["Scripts", folderName]).rightclick();
    cy.sbnContextMenuAction("action-delete").click();
    cy.sbnButtonByLabelKey("general_ok").click();

    cy.contains(folderName).should('not.exist');
    cy.sbnScriptExists(folderName).should('be.false')

    // A script an a folder is the same thing in this case
    cy.sbnEnsureScriptNameNotExists(folderName);
  });
});
